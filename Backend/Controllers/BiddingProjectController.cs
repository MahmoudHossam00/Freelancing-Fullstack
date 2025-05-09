using Freelancing.DTOs.AuthDTOs;
using Freelancing.DTOs.BiddingProjectDTOs;
using Freelancing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingProjectController : ControllerBase
    {
        private readonly IBiddingProjectService _biddingProjectService;
		private readonly UserManager<AppUser> _userManager;
		private readonly INotificationRepositoryService notificationrepo;
        private readonly IConfiguration _configuration;

		public BiddingProjectController(IBiddingProjectService biddingProjectService,UserManager<AppUser>  userManager,INotificationRepositoryService _notificationrepo,IConfiguration configuration)
        {
            _biddingProjectService = biddingProjectService;
            _userManager = userManager;
            notificationrepo = _notificationrepo;
            _configuration = configuration;
		}


        [HttpPost("Filter")]
        public async Task<IActionResult> GetAll([FromBody]BiddingProjectFilterDTO filters, [FromQuery] int pageNumber, [FromQuery] int PageSize)
        {

            return Ok(await _biddingProjectService.GetAllBiddingProjectsAsync(filters, pageNumber, PageSize));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Pass userId to service
            var result = await _biddingProjectService.GetBiddingProjectByIdAsync(id, userId);

            return result != null ? Ok(result) : BadRequest(new { Message = "No Bidding Project Found Hasing This Id" });


            //return Ok(await _biddingProjectService.GetBiddingProjectByIdAsync(id));
        }
        


        //[HttpPost]
        //public async Task<IActionResult> Create(BiddingProjectCreateUpdateDTO p)
        //{
        //    var project = await _biddingProjectService.CreateBiddingProjectAsync(p, User.FindFirstValue(ClaimTypes.NameIdentifier));

        //    return Ok(new {project.ClientId,project.Subcategory.Name,p=project.ProjectSkills.Select(ps=>ps.Skill.Name).ToList()});
        //}

        [HttpPost]
        [Authorize(Roles ="Client")]
        public async Task<IActionResult> Create(BiddingProjectCreateUpdateDTO p)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(((await _userManager.FindByIdAsync(userid))as Client).RemainingNumberOfProjects <=0)
                {
					return BadRequest(new { Message = "You exceeded the number of posts, please wait or subscribe in a plan" });
				}
				var project = await _biddingProjectService.CreateBiddingProjectAsync(p, userid);
                var freelancers= await _userManager.Users.OfType<Freelancer>().ToListAsync();
				foreach (var freelancer in freelancers)
                {
                    await notificationrepo.CreateNotificationAsync(new()
                    {
                        isRead = false,
                        Message = $"New Bidding Project Posted{_configuration["AppSettings:AngularAppUrl"] + $"/details/{project.Id}"}",
                        UserId = freelancer.Id
					});

                }
                if((await _biddingProjectService.GetmyBiddingProjectsAsync(userid, userRole.Client,1,5000)).Count() ==1)
                {
					await notificationrepo.CreateNotificationAsync(new()
					{
						isRead = false,
						Message = $"Congratulations on your first Bidding project Post{_configuration["AppSettings:AngularAppUrl"] + $"/details/{project.Id}"}",
						UserId = userid
					});
				};

				//return Ok(new
				//{
				//    project.ClientId,
				//    project.Subcategory.Name,
				//    p = project.ProjectSkills.Select(ps => ps.Skill.Name).ToList()
				//});
				return Ok(new
                {
                    ClientId = project.ClientId ?? userid,
                    project.Subcategory.Name,
                    project.Id,
                    p = project.ProjectSkills.Select(ps => ps.Skill.Name).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, details = ex.StackTrace });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]BiddingProjectCreateUpdateDTO p)
        {
            var project = await _biddingProjectService.UpdateBiddingProjectAsync(p, id);

            return Ok(new { project.ClientId, project.FreelancerId, project.Id });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _biddingProjectService.DeleteBiddingProjectAsync(id));
        }

        [HttpGet("GetMyBiddingProjects")]
        public async Task<IActionResult> GetMyBiddingProjectsAll([FromQuery] int pageNumber=1, [FromQuery] int PageSize=5)
        {
            userRole role = userRole.Client;
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentuser = await _userManager.FindByIdAsync(userid);
            if(currentuser==null)
            {
                return BadRequest(new { Message = "Current User Not Found" });
            }
            if (currentuser.GetType() == typeof(Client))
            {
                role = userRole.Client;
            }
            else if (currentuser.GetType() == typeof(Freelancer))
            {
                role = userRole.Freelancer;
            }
            else
            {
                BadRequest (new { Message = "User is not a client or freelancer" });
            }

            return Ok(await _biddingProjectService.GetmyBiddingProjectsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), role, pageNumber, PageSize));
        }

		[HttpGet("GetForUser/{userId}")]
		public async Task<IActionResult> GetFreelancerbiddingprojects(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int PageSize = 300)
		{
			userRole role = userRole.Client;
			//var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var currentuser = await _userManager.FindByIdAsync(userId);
			if (currentuser.GetType() == typeof(Client))
			{
				role = userRole.Client;
			}
			else if (currentuser.GetType() == typeof(Freelancer))
			{
				role = userRole.Freelancer;
			}
			else
			{
				BadRequest(new { Message = "User is not a client or freelancer" });
			}

			return Ok(await _biddingProjectService.GetmyBiddingProjectsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), role, pageNumber, PageSize));
		}



		//[HttpPost("Filter")]
		//public async Task<IActionResult> Filter(BiddingProjectFilterDTO pdto, int PageNumber, int PageSize)
		//{
		//    return Ok(await _biddingProjectService.Filter(pdto, PageNumber, PageSize));
		//}
	}
}
