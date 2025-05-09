using Freelancing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerLanguagesController : ControllerBase
    {
        public IFreelancerLanguageService _LanguageService { get; }
        public FreelancerLanguagesController(IFreelancerLanguageService languageService)
        {
            _LanguageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLanguages()
        {
            var languageslist = await _LanguageService.GetAllLanguagesAsync();
            if (languageslist == null )
            {
                return BadRequest(new { Message = "there is no languages found" });
            }
            var languagesDTOlist = languageslist.Select(e => new FreelancerLanguageDTO
            {
                id = e.id,
                Language = e.Language,
                IsDeleted = e.IsDeleted,
                freelancerName = e.freelancer.UserName
            });
            return Ok(languagesDTOlist);
        }

        [HttpGet("freelancer/{username}")]
        public async Task<IActionResult> GetAllLanguagesByFreelancerUserName(string username)
        {
            var languageslist = await _LanguageService.GetLanguagesByFreelancerUserNameAsync(username);
            if (languageslist == null)
            {
                return BadRequest(new { Message = "there is no languages for this freelancer" });
            }
            var languagesDTOlist = languageslist.Select(e => new FreelancerLanguageDTO
            {
                id = e.id,
                Language = e.Language,
                IsDeleted = e.IsDeleted,
                freelancerName = e.freelancer.UserName
            });
            return Ok(languagesDTOlist);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLanguageeByID(int id)
        {
            var selected= await _LanguageService.GetLanguageById(id);
            if (selected == null)
            {
                return BadRequest(new { Message = "can't find a language has this id" });
            }
            var languageDTO = new FreelancerLanguageDTO
            {
                id = selected.id,
                Language = selected.Language,
                IsDeleted = selected.IsDeleted,                
                freelancerName = selected.freelancer.UserName,
            };
            return Ok(languageDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> CreateLanguage([FromBody] CreateFreelancerLanguageDTO languageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(languageDTO);
            }
            var language = new FreelancerLanguage
            {
                Language = languageDTO.Language,             
                IsDeleted = false,
                freelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            var created = await _LanguageService.CreateLanguageAsync(language);
            if (created)
            {
                var createdLanguage = await _LanguageService.GetLanguageById(language.id);
                return CreatedAtAction(nameof(GetLanguageeByID), new { id = createdLanguage.id },
                    new
                    {
                        createdLanguage.id,
                        createdLanguage.Language,
                        createdLanguage.IsDeleted,
                        createdLanguage.freelancer.UserName
                    }
                    );
            }
            return BadRequest(new { Message = "failed to create language" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLanguageeById(int id, [FromBody] CreateFreelancerLanguageDTO languageDTO)
        {
            var selected = await _LanguageService.GetLanguageById(id);
            if (selected != null)
            {
                selected.Language = languageDTO.Language;               
                var updated = await _LanguageService.UpdateLanguageAsync(selected);
                if (updated)
                {
                    return Ok(new { Message = "language updated successfully" });
                }
                return BadRequest(new { Message = "failed to update language" });
            }
            return BadRequest(new { Message = "no language found has this id" });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguageById(int id)
        {
            var selected = await _LanguageService.GetLanguageById(id);
            if (selected != null)
            {
                var deleted = await _LanguageService.DeleteLanguageAsync(id);
                if (!deleted)
                {
                    return BadRequest(new { Message = $"Unable to delete language {id}" });
                }
                return Ok(new { Message = "language marked as deleted successfully" });
            }
            return BadRequest(new { Message = "Unable to find language by this id " });
        }
    }
}
