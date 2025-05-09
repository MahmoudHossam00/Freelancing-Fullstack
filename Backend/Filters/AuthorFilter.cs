using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Freelancing.Filters
{
	public class AuthorFilter(ApplicationDbContext _context, UserManager<AppUser> userManager) : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;
			if (!context.RouteData.Values.TryGetValue("id", out var idValue) || !int.TryParse(idValue.ToString(), out var id))
			{
				context.Result = new BadRequestObjectResult(new { message = "Invalid ID" });
				return;
			}
			var entityType = context.RouteData.Values["controller"]?.ToString();
			string authorId;
			if (entityType.ToLower() == "Proposal".ToLower())
			{

				authorId = _context.Proposals.Find(id).FreelancerId;
			}
			else if (entityType.ToLower() == "BiddingProject".ToLower())
            {

                authorId = _context.biddingProjects.Find(id).ClientId;
            }
            else
			{
				authorId = null;
			}			

			var userid = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = userManager.FindByIdAsync(userid).GetAwaiter().GetResult();
			if (user == null)
			{
				context.Result = new BadRequestObjectResult(new { message = "user not found" });
				return;
			}
			if (user.Id != authorId)
			{
				context.Result = new BadRequestObjectResult(new { message = "only Author is allowed to edit" });
				return;
			}
			base.OnActionExecuting(context);
		}
	}
	public class AuthorAndAdminFilter(ApplicationDbContext _context, UserManager<AppUser> userManager) : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var request = context.HttpContext.Request;
			if (!context.RouteData.Values.TryGetValue("id", out var idValue) || !int.TryParse(idValue.ToString(), out var id))
			{
				context.Result = new BadRequestObjectResult(new { message = "Invalid ID" });
				return;
			}
			var entityType = context.RouteData.Values["controller"]?.ToString();
			string authorId;
			if (entityType.ToLower() == "Proposal".ToLower())
			{

				authorId = _context.Proposals.Find(id).FreelancerId;
			}
            if (entityType.ToLower() == "BiddingProject".ToLower())
            {

                authorId = _context.biddingProjects.Find(id).ClientId;
            }
            else
			{
				authorId = null;
			}
			var userid = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = userManager.FindByIdAsync(userid).GetAwaiter().GetResult();
			if (user == null)
			{
				context.Result = new UnauthorizedObjectResult(new { message = "user not found" });
				return;
			}
			if (user.Id != authorId && !context.HttpContext.User.IsInRole("Admin"))
			{
				context.Result = new UnauthorizedObjectResult(new { message = "only Author and admin are allowed to edit/delete" });
				return;
			}
			base.OnActionExecuting(context);
		}
	}
}
