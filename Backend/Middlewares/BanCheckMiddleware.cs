using System.Security.Claims;

namespace Freelancing.Middlewares
{
    public class BanCheckMiddleware
    {
        private readonly RequestDelegate next;

        public BanCheckMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await next(context);
                return;
            }

            var path = context.Request.Path.Value?.ToLower();
            var allowedPaths = new[] { "/api/auth/login", "/api/auth/logout", "/api/bans/active" };
            if (allowedPaths.Any(p => path.StartsWith(p)))
            {
                await next(context);
                return;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {

                var activeBans = await dbContext.Bans.Where(b => b.BannedUserId == userId
                                && b.BanEndDate > DateTime.UtcNow).Select(b => new { b.Description, b.BanEndDate }).FirstOrDefaultAsync();


                if (activeBans is not null)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        message = "You are banned from using this application",
                        banDescription = activeBans.Description,
                        banEndDate = activeBans.BanEndDate
                    }));
                    return;
                }

            }
            await next(context);

        }
    }
}
