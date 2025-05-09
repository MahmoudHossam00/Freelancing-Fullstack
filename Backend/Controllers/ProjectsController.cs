using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace Freelancing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProjectsController(IProjectService context, UserManager<AppUser> userManager, ApplicationDbContext dbContext) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<List<Project>>> GetAllProjects()
		{
			var projects = await context.GetAllProjectsDtoAsync();
			return Ok(projects);

		}


		[HttpGet("MyProjects")]
		public async Task<ActionResult<List<Project>>> MyProjects()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if(userId is null)
			{
				return BadRequest(new { Message = "Not Found" });
			}
			var user = await userManager.FindByIdAsync(userId);
			var projects = await context.GetAllProjectsDtoAsync();
			if (user is Client)
			{
				projects = projects.Where(p => p.ClientId == userId).ToList();

			}
			else if (user is Freelancer)
			{
				projects = projects.Where(p => p.FreelancerId == userId).ToList();

			}
			else
			{
				return BadRequest(new { Message = "Admins dont have projects" });
			}

			return Ok(projects);

		}

		[HttpGet("numberofclients")]
		[Authorize(Roles = "Freelancer")]
		public async Task<IActionResult> GetClientsNumber()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			//var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var projects = await context.GetAllProjectsAsync();
			var prs = projects.Where(p => p.FreelancerId == userId);
			var clients = projects.Where(p => p.FreelancerId == userId).Select(p => p.ClientId).Distinct().Count();
			var completed = prs.Where(p => p.Status == projectStatus.Completed).Count();
				//pending = prs.Where(p => p.Status == projectStatus.Pending).Count(),
			var working = prs.Where(p => p.Status == projectStatus.Working).Count();
			return Ok(new
			{
				clients,
				completed = completed,
				//pending = prs.Where(p => p.Status == projectStatus.Pending).Count(),
				working = working
			});

		}





		[HttpGet("{id}")]
		public async Task<ActionResult<List<Project>>> GetProjectsById(int id)
		{
			var project = await context.GetProjectDtoByIdAsync(id);
			return Ok(project);
		}



        [HttpGet("Category/{id}")]
        public async Task<ActionResult<List<int>>> GetProjectsByCategoryId(int id)
        {
			var projects = await dbContext.project.Include(p=>p.Subcategory).ThenInclude(c=>c.Category).Where(p => p.Subcategory.CategoryId == id).Select(p=>p.Id).ToListAsync();
            return Ok(projects);
        }

    }
}