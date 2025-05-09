using Freelancing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : ControllerBase
    {
        public IEducationService _EducationService { get; }
        public EducationsController(IEducationService educationService )
        {
            _EducationService = educationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEducations() {
            var educationslist  = await _EducationService.GetAllEducations();            
            if (educationslist == null || educationslist.Count() == 0)
            {
                return BadRequest(new { Message = "there is no educations found" });
            }
            var educationsDTOlist = educationslist.Select(e => new EducationDTO
            {
                Id = e.Id,
                Degree = e.Degree,
                FieldOfStudy = e.FieldOfStudy,
                Institution = e.Institution,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Grade = e.Grade,
                Description = e.Description,
                IsDeleted = e.IsDeleted,
                FreelancerName = e.Freelancer.UserName
            });
            return Ok(educationsDTOlist);
        }

        [HttpGet("freelancer/{username}")]
        public async Task<IActionResult> GetAllEducationsByFreelancerUserName(string username)
        {
            var educationslist = await _EducationService.GetAllEducationsByFreelancerUserName(username);
            if (educationslist == null)
            {
                return BadRequest(new { Message = "there is no educations for this freelancer" });
            }
            var educationsDTOlist = educationslist.Select(e => new EducationDTO
            {
                Id = e.Id,
                Degree = e.Degree,
                FieldOfStudy = e.FieldOfStudy,
                Institution = e.Institution,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Grade = e.Grade,
                Description = e.Description,
                IsDeleted = e.IsDeleted,
                FreelancerName = e.Freelancer.UserName
            });
            return Ok(educationsDTOlist);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEducationByID(int id)
        {
            var selected = await _EducationService.GetEducationById(id);
            if (selected == null)
            {
                return BadRequest(new { Message = "can't find a education has this id" });
            }
            var educationDTO = new EducationDTO
            {
                Id = selected.Id,
                Degree = selected.Degree,
                FieldOfStudy = selected.FieldOfStudy,
                Institution = selected.Institution,
                StartDate = selected.StartDate,
                EndDate = selected.EndDate,
                Grade = selected.Grade,
                Description = selected.Description,
                IsDeleted = selected.IsDeleted,
                FreelancerName = selected.Freelancer.UserName
            };
            return Ok(educationDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> CreateEducation([FromBody] CreateEducationDTO educationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = educationDTO });
            }
            var education = new Education
            {
                Grade = educationDTO.Grade,
                Institution = educationDTO.Institution,
                EndDate = educationDTO.EndDate,
                StartDate = educationDTO.StartDate,
                FieldOfStudy = educationDTO.FieldOfStudy,
                Degree = educationDTO.Degree,
                Description = educationDTO.Description,
                IsDeleted = false,
                FreelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            var created = await _EducationService.AddEducation(education);
            if (created)
            {
                var createdEducation = await _EducationService.GetEducationById(education.Id);
                return CreatedAtAction(nameof(GetEducationByID), new { id = createdEducation.Id },
                    new
                    {
                        createdEducation.Id,
                        createdEducation.Institution,
                        createdEducation.StartDate,
                        createdEducation.EndDate,
                        createdEducation.FieldOfStudy,
                        createdEducation.Degree,
                        createdEducation.Description,
                        createdEducation.Grade,
                        createdEducation.IsDeleted,
                        createdEducation.Freelancer.UserName
                    }
                    );
            }
            return BadRequest(new { Message = "failed to create education" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducationById(int id, [FromBody] UpdateEducationDTO educationDTO)
        {
            var selected = await _EducationService.GetEducationById(id);
            if (selected != null)
            {
                selected.StartDate = educationDTO.StartDate;
                selected.EndDate = educationDTO.EndDate;
                selected.Degree = educationDTO.Degree;
                selected.Description = educationDTO.Description;
                selected.FieldOfStudy = educationDTO.FieldOfStudy;
                selected.Institution = educationDTO.Institution;
                selected.Grade = educationDTO.Grade;
                var updated = await _EducationService.UpdateEducation(selected);
                if (updated)
                {
                    return Ok(new { Message = "education updated successfully" });
                }
                return BadRequest(new { Message = "failed to update education" });
            }
            return BadRequest(new { Message = "no education found has this id" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducationById(int id)
        {
            var selected = await _EducationService.GetEducationById(id);
            if (selected != null)
            {
                var deleted = await _EducationService.DeleteEducation(id);
                if (!deleted)
                {
                    return BadRequest(new { Message = $"Unable to delete education {id}" });
                }
                return Ok(new { Message = "education marked as deleted successfully" });
            }
            return BadRequest(new { Message = "Unable to find education by this id " });
        }
    }
}
