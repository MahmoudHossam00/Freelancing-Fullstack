using AutoMapper;
using Freelancing.DTOs;
using Freelancing.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BansController : ControllerBase
    {
        private readonly IBanRepositoryService banService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public BansController(IBanRepositoryService banService, IMapper mapper , ApplicationDbContext context)
        {
            this.banService = banService;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BanDto>> GetBan(int id)
        {

            var ban = await banService.GetBanByIdAsync(id);
            if (ban == null)
            {
                return BadRequest(new { Message ="No Bans Found"});
            }
            var banDto = mapper.Map<BanDto>(ban);
            return Ok(banDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<BanDto>>> GetBansByUserId(string userId)
        {
            var bans = await banService.GetBansByUserIdAsync(userId);
            if (bans == null)
            {
                return BadRequest(new { Message = "No Bans Found" });
            }
            var banDtos = mapper.Map<List<BanDto>>(bans);
            return Ok(banDtos);
        }

        [HttpPost]
        public async Task<ActionResult<BanDto>> CreateBan([FromBody] BanDto banDto)
        {
            if (banDto == null)
                return BadRequest(new { Message = "BanDto cannot be null" });

            var userExists = await context.Users.AnyAsync(u => u.Id == banDto.BannedUserId);
            if (!userExists)
                return BadRequest(new { Message = "Invalid BannedUserId" });

            var ban = mapper.Map<Ban>(banDto);

            var createdBan = await banService.CreateBanAsync(ban);
            if (createdBan == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create ban");

            var createdBanDto = mapper.Map<BanDto>(createdBan);

            return CreatedAtAction(
                nameof(GetBan),
                new { id = createdBanDto.Id },
                createdBanDto
            );
        }
        [HttpPut("{id}")]




        public async Task<ActionResult<BanDto>> UpdateBan(int id, [FromBody] BanDto banDto)
        {
            if (banDto is null)
                return BadRequest(new { Message = "BanDto cannot be null" });
            var existingBan = await banService.GetBanByIdAsync(id);
            if (existingBan == null)
            {
                return BadRequest(new { Message = "No Bans Found" });
            }
            mapper.Map(banDto, existingBan);
            //updatedBan.Id = id;
            await banService.UpdateBanAsync(existingBan);
            var updatedBanDto = mapper.Map<BanDto>(existingBan);
            return Ok(updatedBanDto);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBan(int id)
        {
            var existingBan = await banService.GetBanByIdAsync(id);
            if (existingBan == null)
            {
                return BadRequest(new { Message ="No Bans Found"});
            }
            await banService.DeleteBanAsync(id);
            return NoContent();
        }

        [HttpGet("active/{userId}")]
        public async Task<ActionResult<BanDto>> GetActiveBan(string userId)
        {
            var ban = await banService.GetActiveBansByUserIdAsync(userId);
            if (ban == null)
            {
                return BadRequest(new { Message = "No active ban found for this user." });
            }
            return Ok(mapper.Map<BanDto>(ban));
        }


        [HttpGet("banned-users")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<BanDto>>> GetBannedUsers()
        {
            var bannedUsers = await banService.GetBannedUsersAsync();
            if (bannedUsers.Count()==0)
            {
                return BadRequest(new { Message = "No banned users found." });
            }
            var bannedUserDtos = mapper.Map<List<BanDto>>(bannedUsers);
            return Ok(bannedUserDtos);
        }
    }
}
