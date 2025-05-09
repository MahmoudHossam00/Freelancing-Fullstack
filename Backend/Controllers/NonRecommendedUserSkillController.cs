using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonRecommendedUserSkillController(ApplicationDbContext context) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string SkillName)
        {
            NonRecommendedUserSkill NewSkill = new NonRecommendedUserSkill() { Name = SkillName };
            if (!context.nonRecommendedUserSkills.Any(s => s.Name == SkillName))
                context.nonRecommendedUserSkills.Add(NewSkill);
            else
            {
                NewSkill = context.nonRecommendedUserSkills.FirstOrDefault(s => s.Name == SkillName);
            }
            context.SaveChanges();
			var freelancer = context.freelancers.Include(f=>f.NonRecommendedUserSkills).FirstOrDefault(f => f.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (freelancer is not null)
            {

               freelancer.NonRecommendedUserSkills.Add(NewSkill);
                await context.SaveChangesAsync();
                return Ok(new { NewSkill.Id, NewSkill.Name });
            }
            return BadRequest(new { Message = "Freelancer not found" });
        }
		[Authorize]
		[HttpGet("byuser")]
		public async Task<IActionResult> getbyuserid()
        {
            return Ok(context.nonRecommendedUserSkills.Include(f => f.Freelancers).Where(s => s.Freelancers.Any(f => f.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))&&!s.IsDeleted).Select(s => new { s.Id,s.Name}).ToList());
        }
        [HttpDelete("{SkillId}")]
        public async Task<IActionResult> Delete(int SkillId)
        {
            var freelancer = context.freelancers.Include(f=>f.NonRecommendedUserSkills).FirstOrDefault(f => f.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (freelancer is not null)
            {
                var skill = freelancer.NonRecommendedUserSkills.FirstOrDefault(s => s.Id == SkillId);
                if (skill is not null)
                {
                    freelancer.NonRecommendedUserSkills.Remove(skill);
                    await context.SaveChangesAsync();
                    return Ok(new { Message = "Skill removed successfully" });
                }
                return BadRequest(new { Message = "Skill not found" });

            }
            return BadRequest(new { Message = "Freelancer not found" });

        }


    }
}
