using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Freelancing.DTOs;
using Freelancing.Models;
using AutoMapper;

using System.Threading.Channels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortofolioProjectController : ControllerBase
    {
        private readonly IPortofolioProject portofolioProjectContext;
        private readonly IMapper mapper;

        public PortofolioProjectController(IPortofolioProject portofolioProject, IMapper mapper)
        {
            this.mapper = mapper;
            portofolioProjectContext = portofolioProject;
        }

        // GET: api/PortofolioProject
        [HttpGet]
        public async Task<IActionResult> GetPortofolioProjects()
        {
            var p = await portofolioProjectContext.GetAllAync();
            if (p == null)
            {
                return BadRequest(new { Message = "Not Found" });
            }
            var DTO = mapper.Map<List<PortofolioProjectDTO>>(p);
            return Ok(DTO);
        }

		[HttpGet("MyPortofolioProjects")]
		public async Task<IActionResult> GetmyPortofolioProjects()
		{
			var p = await portofolioProjectContext.GetByFreelancerId(User.FindFirstValue(ClaimTypes.NameIdentifier));
			if (p == null)
			{
				return BadRequest(new {Message="Only for freelancers"});
			}
			var DTO = mapper.Map<List<PortofolioProjectDTO>>(p);
			return Ok(DTO.Select(p=> new
            {
                p.Id,
                p.Description,
                p.FreelancerId,
                p.Title,
                p.CreatedAt,
                Images=p.Images.Select(i => new {i.Image,i.Id,i.PreviousProjectId})
			}));
		}


		[HttpGet("UserPortofolioProjects/{userId}")]
		public async Task<IActionResult> GetuserPortofolioProjects(string userId)
		{
            
			var p = await portofolioProjectContext.GetByFreelancerId(userId);
			if (p == null)
			{
				return BadRequest(new { Message = "Not Found" });
			}
			var DTO = mapper.Map<List<PortofolioProjectDTO>>(p);
			return Ok(DTO.Select(p => new
			{
				p.Id,
				p.Description,
				p.FreelancerId,
				p.Title,
				p.CreatedAt,
				Images = p.Images.Select(i => new { i.Image, i.Id, i.PreviousProjectId })
			}));
		}

		// GET: api/PortofolioProject/5
		[HttpGet("{id}")]
        public async Task<IActionResult> GetPortofolioProjecBytId(int id)
        {
            var p = await portofolioProjectContext.GetByIdAsync(id);

            if (p == null)
            {
                return BadRequest(new { Message = "Not Found" });
            }

            var DTO = mapper.Map<PortofolioProjectDTO>(p);
            return Ok(DTO);
        }

        // PUT: api/PortofolioProject/5
        //needs update bs kman shwaya
        [HttpPut("{id}")]
        [Authorize(Roles ="Freelancer")]
        public async Task<IActionResult> Update(int id, [FromBody] TempModel portofolioProjectDTO)
        {
            //if (id != portofolioProjectDTO.Id)
            //{
            //    return BadRequest(new { Message ="Not Found"});
            //}

            //find the project
            var existingProject = await portofolioProjectContext.GetByIdAsync(id);
            if (existingProject == null)
            {
                return BadRequest(new { Message = $"No project found with ID = {id}" });
            }


            //existingProject.Title = portofolioProjectDTO.Title;
            //existingProject.Description=portofolioProjectDTO.Description;
            //existingProject.CreatedAt = DateTime.UtcNow;  
            //existingProject.FreelancerId=portofolioProjectDTO.FreelancerId;

            //prepare it to be updated (mapped it)
            mapper.Map<PortofolioProjectDTO>(existingProject);

            //update it
            var updatedProject = await portofolioProjectContext.UpdateAsync(new PortofolioProjectDTO()
            {
                CreatedAt= portofolioProjectDTO.CreatedAt,
                Description= portofolioProjectDTO.Description,
                Title= portofolioProjectDTO.Title,
                Id=id,
                FreelancerId=User.FindFirstValue(ClaimTypes.NameIdentifier)
			});

            //to be sent back to respone as dto
            var updatedDTO = mapper.Map<PortofolioProjectDTO>(updatedProject);

            return Ok(portofolioProjectDTO);


        }

        //  POST: api/PortofolioProject
        [HttpPost]
        [Authorize(Roles ="Freelancer")]
        public async Task<ActionResult<PortofolioProjectDTO>> Create([FromForm] CreatePortfolioProjectDTO portofolioProjectDTO)
        {
            var createdProject = await portofolioProjectContext.AddAsync(portofolioProjectDTO,User.FindFirstValue(ClaimTypes.NameIdentifier));

            var dto = mapper.Map<PortofolioProjectDTO>(createdProject);

            return Ok(new {Message="Created"});
        }

        // DELETE: api/PortofolioProject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortofolioProject(int id)
        {
            var success = await portofolioProjectContext.DeleteAsync(id);
            if (!success)
            {
                return BadRequest(new { Message = $"No project found with ID = {id}" });
            }

            return NoContent();
        }

    }

	public class TempModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}