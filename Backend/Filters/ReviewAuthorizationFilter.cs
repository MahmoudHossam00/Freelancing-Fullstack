using Freelancing.Models;
using Freelancing.IRepositoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Freelancing.Filters
{
    public class ReviewAuthorizationFilter : IAsyncActionFilter
    {
        private readonly IReviewRepositoryService _reviewService;
        private readonly UserManager<AppUser> _userManager;

        public ReviewAuthorizationFilter(IReviewRepositoryService reviewService, UserManager<AppUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var action = context.ActionDescriptor.RouteValues["action"];
            var id = context.RouteData.Values["id"]?.ToString();

            if (action == "UpdateReview" || action == "DeleteReview")
            {
                var review = await _reviewService.GetReviewByIdAsync(int.Parse(id));
                if (review == null)
                {
                    context.Result = new NotFoundResult();
                    return;
                }

                if (review.ReviewerId != userId && !context.HttpContext.User.IsInRole("Admin"))
                {
                    context.Result = new BadRequestObjectResult(new { message = "You are not authorized to perform this action" });
                    return;
                }
            }

            await next();
        }
    }
}
