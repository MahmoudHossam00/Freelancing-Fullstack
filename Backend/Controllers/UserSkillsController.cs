using AutoMapper;
using Freelancing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserSkillsController:ControllerBase
    {
        private readonly IUserSkillService skillService;
        private readonly IMapper mapper;

        public UserSkillsController(IUserSkillService skillService ,IMapper mapper)
        {
            this.skillService = skillService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserSkills()
        {
            var userSkills = await skillService.GetAllUserSkillAsync();
            var userSkillDtos = mapper.Map<List<UserSkillDto>>(userSkills);
            return Ok(userSkillDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSkillById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid user skill ID" });
            var userSkill = await skillService.GetUserSkillByIDAsync(id);
            if (userSkill == null)
                return BadRequest(new { Message = $"User skill with ID {id} not found" });
            return Ok(mapper.Map<UserSkillDto>(userSkill));
        }

        //[HttpPost]
        //public async Task<ActionResult<UserSkillDto>> CreateUserSkill([FromBody] UserSkillDto userSkillCreateDto)
        //{
        //    if (userSkillCreateDto == null || !ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var userSkill = mapper.Map<UserSkill>(userSkillCreateDto);
        //    var createdUserSkill = await skillService.CreateUserSkillAsync(userSkill);
        //    var userSkillDto = mapper.Map<UserSkillDto>(createdUserSkill);
        //    return CreatedAtAction(nameof(GetUserSkillById), new { id = userSkillDto.SkillId }, userSkillDto);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<SkillDto>>  UpdateUserSkill(int id, [FromBody] UserSkillDto userSkillUpdateDto)
        //{
        //    if (id <= 0 || userSkillUpdateDto == null || !ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var userSkill = mapper.Map<UserSkill>(userSkillUpdateDto);
        //    userSkill.id = id;
        //    var updatedUserSkill = await skillService.UpdateUserSkillAsync(userSkill);
        //    if (updatedUserSkill == null)
        //        return NotFound($"User skill with ID {id} not found");
        //    return Ok(mapper.Map<UserSkillDto>(updatedUserSkill));
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserSkill(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest("Invalid user skill ID");
        //    var isDeleted = await skillService.DeleteUserSkillAsync(id);
        //    if (!isDeleted)
        //        return NotFound($"User skill with ID {id} not found");
        //    return NoContent();
        //}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSkillByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "Invalid user ID" });
            var userSkills = await skillService.GetUserSkillByUserIdAsync(userId);
            if (userSkills == null)
                return BadRequest(new { Message = $"No user skills found for user ID {userId}" });
            var userSkillDtos = mapper.Map<List<UserSkillDto>>(userSkills);
            return Ok(userSkillDtos);
        }
    }
}
