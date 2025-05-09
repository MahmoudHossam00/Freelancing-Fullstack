using Freelancing.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;
using Freelancing.Filters;
using Freelancing.SignalR;
using Microsoft.OpenApi.Models;
using Freelancing.Services;
using Microsoft.Extensions.Options;
using static Freelancing.Models.Stripe;
using Freelancing.Helpers;
using Microsoft.AspNetCore.SignalR;

using Microsoft.ML;

using static Freelancing.SignalR.ChatHub;
using DotNetEnv;



namespace Freelancing
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddSignalR();
            #region AddCors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
					policy
                         //.AllowAnyOrigin()
                         .WithOrigins(
                "http://localhost:4200",
                "http://localhost:60663",
                "http://127.0.0.1:4200"
            )

                        //  .WithOrigins("http://127.0.0.1:4200")
                        .AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
                        //.SetIsOriginAllowed(_ => true); 
                });
            });
            #endregion
            #region Configuring identity
             builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			builder.Services.AddAuthentication((options) =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer((options) =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
					ClockSkew = TimeSpan.Zero


				};
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						if (context.Request.Path.StartsWithSegments("/chathub")|| context.Request.Path.StartsWithSegments("/notification")|| context.Request.Path.StartsWithSegments("/biddingHub"))
						{
							var accessToken = context.Request.Query["access_token"];
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			}).AddCookie(

				options => options.Events = new CookieAuthenticationEvents
				{
					OnRedirectToLogin = context =>
					{
						if (context.Request.Path.StartsWithSegments("/api"))
						{
							context.Response.StatusCode = 401;
							context.Response.ContentType = "application/json";
							return Task.CompletedTask;
						}

						context.Response.Redirect(context.RedirectUri);
						return Task.CompletedTask;
					}
				})
				.AddGoogle(OP =>
				{
					OP.ClientId = builder.Configuration["Authentication:Google:ClientId"];
					OP.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

				}).AddFacebook(op =>
				{
					op.AppId = "1341703253547842";
					op.AppSecret = "9b2593ba5d2a3d86957ec93831791298";
				});

		

			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.SignIn.RequireConfirmedEmail = true; 
			});
			builder.Services.AddAuthorization();
			#endregion
			#region services

			builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen(c =>
			//{
			//    c.MapType<IFormFile>(() => new OpenApiSchema
			//    {
			//        Type = "string",
			//        Format = "binary"
			//    });
			//});
			builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
			builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<StripeSettings>>().Value);
			builder.Services.AddHttpClient();

			builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

			builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Enable JWT Auth in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });



			builder.Services.AddLogging();
            builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
            builder.Services.AddScoped<IPortofolioProjectImage, PortofolioProgectImageService>();
            builder.Services.AddScoped<CloudinaryService>();

            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReviewRepositoryService, ReviewRepositoryService>();
            builder.Services.AddScoped<IChatRepositoryService, ChatRepositoryService>();
            builder.Services.AddScoped<IBanRepositoryService, BanRepositoryService>();
            builder.Services.AddScoped<INotificationRepositoryService, NotificationRepositoryService>();
            builder.Services.AddScoped<IMilestoneService, MilestoneService>();
            builder.Services.AddScoped<IFixedProjectService, FixedProjectService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();


			builder.Services.AddScoped<IChatRepositoryService, ChatRepositoryService>();
			builder.Services.AddScoped<IBanRepositoryService, BanRepositoryService>();
			builder.Services.AddScoped<INotificationRepositoryService, NotificationRepositoryService>();
			builder.Services.AddScoped<IMilestoneService, MilestoneService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();
			builder.Services.AddScoped<IEducationService, EducationService>();
			builder.Services.AddScoped<IExperienceService, ExperienceService>();
			builder.Services.AddScoped<IEmailSettings, EmailSettings>();
			builder.Services.AddScoped<ICertificatesService, CertificateService>();
			builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
			builder.Services.AddScoped<INotificationRepositoryService, NotificationRepositoryService>();
			builder.Services.AddScoped<IFreelancerService, FreelancerService>();
			builder.Services.AddScoped<IClientService, ClientService>();
			builder.Services.AddScoped<IFreelancerLanguageService, FreelancerLanguageService>();
			builder.Services.AddScoped<IBiddingProjectService, BiddingProjectService>();
			builder.Services.AddScoped<IproposalService, ProposalService>();
			builder.Services.AddScoped<IProjectService, ProjectService>();
			builder.Services.AddScoped<IPortofolioProject,PortofolioProjectService>();
            builder.Services.AddScoped<ISkillService, SkillService>();
            builder.Services.AddScoped<IUserSkillService, UserSkillService>();
            builder.Services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<ICityService, CityService>();
			builder.Services.AddHttpContextAccessor();

			#endregion

			#region Filters
			builder.Services.AddScoped<AuthorFilter>();
			builder.Services.AddScoped<AuthorAndAdminFilter>();

			builder.Services.AddScoped<ReviewAuthorizationFilter>();
			#endregion


			builder.Services.AddAutoMapper(typeof(ReviewProfile), typeof(BanProfile), typeof(NotificationProfile), typeof(ChatProfile) ,typeof(SkillProfile) ,typeof(UserSkillProfile),typeof(ProjectSkillProfile));
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            //builder.Services.AddHttpClient<IAIService, AIService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			builder.Services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), ImageSettings.ImagesPath);
			if (!Directory.Exists(uploadsPath))
			{
				Directory.CreateDirectory(uploadsPath); // Ensure the directory exists
			}
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(uploadsPath),
				RequestPath = "/files"
			}); //enabling wwwroot with the localhost/files url 
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")),
				RequestPath = "/projectimages"
			});

			using (var scope = app.Services.CreateScope())//seeding roles and a user
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

				await RoleSeeder.SeedRolesAsync(roleManager, userManager);
			
			}


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            app.UseCors("AllowAll");
			//app.UseHttpsRedirection();


			app.Use(async (context, next) =>
			{
				if (context.Request.Method == HttpMethods.Options)
				{
					context.Response.StatusCode = StatusCodes.Status200OK;
					await context.Response.CompleteAsync();
					return;
				}
				await next();
			});
			app.UseAuthentication();
			app.UseAuthorization();


			app.UseMiddleware<BanCheckMiddleware>();
			app.MapControllers();
			app.MapHub<ChatHub>("/chathub");
			app.MapHub<NotificationHub>("/notification");
			app.MapHub<BiddingHub>("/biddingHub");
			//app.MapHub<NotificationHub>("/chathub");
			app.Run();
		}
	}
}
