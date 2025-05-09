using Freelancing.DTOs.ProposalDTOS;
using Freelancing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace Freelancing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WishlistController(INotificationRepositoryService _notifications,ApplicationDbContext _context) : ControllerBase
	{
		[HttpGet]
		//[Authorize(Roles = "Freelancer")]
		public IActionResult GetmyWishlist()
		{
			// Logic to get the wishlist
			var wishlist = _context.FreelancerWishlists.Include(P => P.Project).Include(p => p.Freelancer).Where(f => f.FreelancerId == User.FindFirstValue(ClaimTypes.NameIdentifier));
			
			return Ok(
				wishlist.Select(w => new
				{
					w.ProjectId,
					w.Project.Title,
					currency=w.Project.currency.ToString(),
					w.Project.CreatedAt,
					w.Project.experienceLevel,
					price = w.Project is FixedPriceProject
			 ? (w.Project as FixedPriceProject).Price
			 : w.Project is BiddingProject
			   ? (w.Project as BiddingProject).BidCurrentPrice
			   : 0,
			   type= w.Project is FixedPriceProject
			 ? "FixedPrice": "Bidding"
			  
				}
				)


				);
		}
		
		
		
		
		
		
		[HttpPost("{projectid}")]
		[Authorize(Roles = "Freelancer")]
		public async Task<IActionResult> AddToWishlist(int projectid)
		{
			// Logic to get the wishlist
			var project = _context.project.Include(p=>p.Freelancer).FirstOrDefault(p => p.Id == projectid);
			if (project == null)
			{
				return BadRequest(new { Message = "Project not found" });
			}
			//if(! (project.Status==projectStatus.Completed))
			//{
			//	return BadRequest(new { message = "Project is not  completed" });
			//}
			if (_context.FreelancerWishlists.Any(f => f.FreelancerId == User.FindFirstValue(ClaimTypes.NameIdentifier) && f.ProjectId == projectid))
			{
				return BadRequest(new { Message = "Project already in wishlist" });
			}
			var wishlist = new FreelancerWishlist()
			{
				FreelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
				ProjectId = projectid
			};


			_context.FreelancerWishlists.Add(wishlist);


			_context.SaveChanges();
			project = _context.project.Include(p => p.Freelancer).FirstOrDefault(p => p.Id == projectid);
			//User.FindFirstValue(ClaimTypes.Name);
			await _notifications.CreateNotificationAsync(new()
			{
				isRead = false,
                Message = $"your project with id {project.Id} was added to wishwishlist  by userid {User.FindFirstValue(ClaimTypes.Name)}",
                UserId = project.ClientId
			});
			return Ok(new { id = wishlist.Id });
		}




		[HttpDelete("{projectid}")]
		[Authorize(Roles = "Freelancer")]
		public IActionResult RemoveFromWishlist(int projectid)
		{
			// Logic to get the wishlist
			var project = _context.project.FirstOrDefault(p => p.Id == projectid);
			if (project == null)
			{
				return BadRequest(new { Message = "Project not found" });
			}
			//if (!(project.Status == projectStatus.Completed))
			//{
			//	return BadRequest(new { message = "Project is not  completed" });
			//}
			
			var wishlistitem = _context.FreelancerWishlists.FirstOrDefault(f => f.FreelancerId == User.FindFirstValue(ClaimTypes.NameIdentifier) && f.ProjectId == projectid);
			if(wishlistitem==null)
			{
				return BadRequest(new { Message = "Project is not in wishlist" });
			}
			_context.FreelancerWishlists.Remove(wishlistitem);
			_context.SaveChanges();
			return Ok(new { Message = "Removed" });
		}
	}
}
