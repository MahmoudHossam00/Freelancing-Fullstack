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
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificatesService _certificatesService;

        public CertificatesController(ICertificatesService certificatesService)
        {
            _certificatesService = certificatesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCertificates()
        {
            var certificateslist = await _certificatesService.GetAllUserCertificatesAsync();
            if (certificateslist == null )
            {
                return BadRequest(new { Message = "there is no certificates found" });
            }
            var certificatesDTOlist = certificateslist.Select(e => new CertificateDTO
            {
                Id = e.Id,
                Name = e.Name,
                IssueDate = e.IssueDate,
                IsDeleted = e.IsDeleted,
                issuer=e.Issuer,
                FreelancerName = e.Freelancer.UserName
            });
            return Ok(certificatesDTOlist);
        }
        
        [HttpGet("freelancer/{username}")]
        public async Task<IActionResult> GetAllCertificatesByFreelancerUserName(string username)
        {
            var certificateslist = await _certificatesService.GetAllCertificatesByFreelancerUserName(username);
            if (certificateslist == null)
            {
                return BadRequest(new {Message = "there is no certificates for this freelancer"});
            }
            var certificatesDTOlist = certificateslist.Select(e => new CertificateDTO
            {
                Id = e.Id,
                Name = e.Name,
                IssueDate = e.IssueDate,
                IsDeleted = e.IsDeleted,
				issuer = e.Issuer,
				FreelancerName = e.Freelancer.UserName
            });
            return Ok(certificatesDTOlist);
        }

        [HttpGet("freelancer")]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> GetAllCertificatesByFreelancerId()
        {

            var freelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var certificateslist = await _certificatesService.GetAllCertificatesByFreelancerId(freelancerId);
            if (certificateslist == null)
            {
                return BadRequest(new { Message = "there is no certificates for this freelancer" });
            }
            var certificatesDTOlist = certificateslist.Select(e => new CertificateDTO
            {
                Id = e.Id,
                Name = e.Name,
                IssueDate = e.IssueDate,
                IsDeleted = e.IsDeleted,
            }
            );
            return Ok(certificatesDTOlist);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificateByID(int id) {
            var selectedcertificate =await _certificatesService.GetCertificateByIDAsync(id);
            if (selectedcertificate == null)
            {
                return BadRequest(new {Message ="can't find a certificate has this id"});
            }
            var certificateDTO = new CertificateDTO {
                Name = selectedcertificate.Name,
                IsDeleted = selectedcertificate.IsDeleted,
                IssueDate = selectedcertificate.IssueDate,
				issuer = selectedcertificate.Issuer,
				FreelancerName = selectedcertificate.Freelancer.UserName,
            };
            return Ok(certificateDTO);
        }


        [HttpPost]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> CreateCertificate([FromBody] CreateCertificateDTO certificateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = certificateDTO });
            }
            var certificate = new Certificate
            {
                Name = certificateDTO.Name,
                IssueDate = certificateDTO.IssueDate,
                IsDeleted = false,
				Issuer = certificateDTO.Issuer,
				FreelancerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            var created = await _certificatesService.CreateCertificateAsync(certificate);
            if (created)
            {
                var createdCertificate = await _certificatesService.GetCertificateByIDAsync(certificate.Id);
                return CreatedAtAction(nameof(GetCertificateByID), new { id = createdCertificate.Id },
                    new
                    {
                        createdCertificate.Id,
                        createdCertificate.Name,
						createdCertificate.Issuer,

						createdCertificate.IssueDate,
                        createdCertificate.IsDeleted,
                        createdCertificate.Freelancer.UserName                                                
                    }
                    );
            }
            return BadRequest(new { Message = "failed to create Certificate" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificateById(int id,[FromBody]CreateCertificateDTO certificateDTO)
        {
            var selected = await _certificatesService.GetCertificateByIDAsync(id);
            if (selected != null)
            {
                selected.IssueDate = certificateDTO.IssueDate;
                selected.Name = certificateDTO.Name;
                selected.Issuer = certificateDTO.Issuer;

				var updated = await _certificatesService.UpdateCertificateAsync(selected);
                if (updated)
                {
                return Ok(new { Message = "certificate updated successfully"});
                }
                return BadRequest(new { Message = "failed to update certificate" });
            }
            return BadRequest(new { Message = "no certificate found has this id" });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificateById (int id)
        {
             var selected =await _certificatesService.GetCertificateByIDAsync(id);
            if (selected != null) {
                var deleted = await _certificatesService.DeleteCertificateAsync(id);
                if (!deleted)
                {
                    return BadRequest(new { Message = $"Unable to delete certificate {id}" });
                }
                return Ok(new { Message = "certificate marked as deleted successfully"});
            }
            return BadRequest(new { Message = "Unable to find certificate by this id " });
        }
    }
}
