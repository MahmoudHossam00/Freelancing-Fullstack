using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribtionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubscribtionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint to get current subscription plan of the user
        [HttpGet("current-plan")]
       
        public IActionResult GetCurrentSubscription()
        {
            // Retrieve the UserId from the claims (assuming 'ClientType' stores the UserId)
            //  var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated or ClientType claim is missing.");
            }
            var userSubscription = _context.UserSubscriptionPlanPayments
                .Include(u => u.SubscriptionPlan) // Join with the SubscriptionPlan
                .Where(u => u.UserId == userId && !u.isDeleted) // Ensure the subscription is not deleted
                .OrderByDescending(u => u.SubscriptionPayment.Date) // Assuming the most recent payment shows current plan
                .FirstOrDefault();

            if (userSubscription == null)
            {
                return NotFound(new { Message = "User doesn't have an active subscription." });
            }

            return Ok(new
            {
                userSubscription.SubscriptionPlan.Id,
                userSubscription.SubscriptionPlan.name,
                userSubscription.SubscriptionPlan.Description,
                userSubscription.SubscriptionPlan.Price,
                userSubscription.SubscriptionPlan.DurationInDays,
                userSubscription.SubscriptionPlan.TotalNumber
            });
        }
    }
}

