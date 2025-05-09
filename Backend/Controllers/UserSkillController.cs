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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSkillController(IUserSkillService context, IMapper mapper) : ControllerBase
    {

        // GET: api/UserSkill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSkillDto>>> GetUserSkills()
        {
           //  var freelancerId = User.Identity.Name;

           var freelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            if (string.IsNullOrEmpty(freelancerId))
            {
                return Unauthorized("Freelancer ID not found in token.");
            }

            var skills = await context.GetUserSkillByUserIdAsync(freelancerId);
          //  var skills =await context.GetAllUserSkillAsync();
            if (skills == null )
            {
                return BadRequest(new { Message = "No skills found" });
            }

            var skillDtos = mapper.Map<List<UserSkillDto>>(skills);
            return Ok(skillDtos);

        }

        // GET: api/UserSkill/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSkillDto>> GetUserSkillById(int id)
        {
            var userSkillDto = await context.GetUserSkillByIDAsync(id); 
            
            if (userSkillDto == null)
            {
                return BadRequest(new { Message = "Not found" });
            }

            var skillDtos = mapper.Map<UserSkillDto>(userSkillDto);
            return Ok(skillDtos);
        }

        // PUT: api/UserSkill/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserSkill(int id, UserSkillDto userSkillDto)
        {
            var freelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // el freelancer el logged

            var userSkill = mapper.Map<UserSkill>(userSkillDto);
            userSkill.id = id;  // Ensure we pass the ID to the service method

            var updatedUserSkill = await context.UpdateUserSkillAsync(userSkill, freelancerId);
            if (updatedUserSkill == null)
            {
                return Unauthorized();  // User cannot update someone else's skill
            }

            if (id != userSkillDto.Id)
            {
                return BadRequest(new { Message = "ID mismatch between route and body." });
            }


            return Ok(updatedUserSkill);

        }

        // POST: api/UserSkill
        [HttpPost]
        [Authorize(Roles ="Freelancer")]
        public async Task<ActionResult<UserSkillDto>> AddUserSkill(UserSkillDto userSkillDto)
        { 
            var userSkill = mapper.Map<UserSkill>(userSkillDto);
            userSkill.FreelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userSkill.SkillId = userSkillDto.Id;
			var NewUserSkill = await context.CreateUserSkillAsync(userSkill);
            var NewUserSkillDto = mapper.Map<UserSkillDto>(NewUserSkill);

            return CreatedAtAction("GetUserSkillById", new { id = NewUserSkillDto.Id }, NewUserSkillDto);
        }

        // DELETE: api/UserSkill/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSkillDto(int id)
        {
            // var userSkillDto = await context.GetUserSkillByIDAsync(id);
            // if (userSkillDto == null)
            // {
            //     return NotFound();
            // }

            //await context.DeleteUserSkillAsync(id);

            var freelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier);  
            var isDeleted = await context.DeleteUserSkillAsync(id, freelancerId);

            if (!isDeleted)
            {
                return Unauthorized();  // User cannot delete someone else's skill
            }

            return NoContent();
        }

    }
}
