using Freelancing.DTOs.MilestoneDTOs;
using Freelancing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilestoneController : ControllerBase
    {
        private readonly IMilestoneService _milestoneService;
        private readonly INotificationRepositoryService _notifications;
        private readonly IConfiguration _configuration;
		private readonly IProjectService _projects;
		public MilestoneController(IProjectService projects,IMilestoneService milestoneService, INotificationRepositoryService notifications,IConfiguration configuration)
        {
            _milestoneService = milestoneService;
            _notifications = notifications;
            _configuration = configuration;
            _projects = projects;
		}


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _milestoneService.GetAllAsync());
        }



        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _milestoneService.GetByIdAsync(id));
        }


        [HttpGet("Project/{ProjectId}")]
        public async Task<IActionResult> GetByProjectId(int ProjectId)
        {
            return Ok(await _milestoneService.GetByProjectId(ProjectId));
        }

      



        [HttpPost]
        public async Task<IActionResult> Post(MilestoneCreateDTO milestoneDto)
        {

            return Ok(await _milestoneService.CreateAsync(milestoneDto));
        }




        
        [HttpPatch("UpdateStatus/{MilestoneId}")]
        public async Task<IActionResult> Patch(int MilestoneId, [FromBody]int StatusId)
        {

            var updatedmilestones=await _milestoneService.UpdateStatusAsync(MilestoneId, StatusId);

            var project=await _projects.GetProjectByIdAsync(updatedmilestones.ProjectId);
			await _notifications.CreateNotificationAsync(new()
			{
				isRead = false,
				Message = $"Milestone Finished for the project `{project.Title}` please proceed for the next milestone",
				UserId = project.FreelancerId
			});
			return Ok(updatedmilestones);
        }


        [HttpPut]
        public async Task<IActionResult> Put(MilestoneCreateDTO milestone)
        {
            return Ok(await _milestoneService.UpdateAsync(milestone));
        }




       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _milestoneService.DeleteAsync(id));
        }



        [HttpPost("UploadMilestoneFiles/{MilestoneId}")]
        public async Task<IActionResult> UploadMilestoneFiles([FromForm] List<IFormFile> files, int MilestoneId)
        {
            //await _milestoneService.UploadFile(files, MilestoneId);
            var project = await _projects.GetProjectByIdAsync((await _milestoneService.GetByIdAsync(MilestoneId)).ProjectId);
			await _notifications.CreateNotificationAsync(new()
			{
				isRead = false,
				Message = $"Milestone Filed uploaded for the project `{project.Title}` please check it",
				UserId = project.ClientId
			});
			return Ok(await _milestoneService.UploadFile(files, MilestoneId));
        }



        [HttpDelete("RemoveMilestoneFiles/{FileId}")]
        public async Task<IActionResult> RemoveMilestoneFiles(int FileId)
        {

            return Ok(await _milestoneService.RemoveFile(FileId));
        }
		[HttpDelete("RemoveMilestoneFilesByName/{Name}")]
		public async Task<IActionResult> RemoveMilestoneFilesByName(string Name)
		{

			return Ok(await _milestoneService.RemoveFilebyName(Name));
		}


		[HttpGet("GetFilesByMilestoneId/{MilestoneId}")]
        public async Task<IActionResult> GetFilesByMilestoneId(int MilestoneId)
        {
            return Ok((await _milestoneService.GetFilesByMilestoneId(MilestoneId)).Select(f=>new { f.id,f.fileName }));
        }


    }
}
