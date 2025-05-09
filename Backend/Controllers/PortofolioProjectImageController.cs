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
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortofolioProjectImageController(IPortofolioProjectImage context, IMapper mapper, IPortofolioProject projectRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetImageByProjectId(int previousProjectId)
        {
            var images = await context.GetByPortfolioProjectIdAsync(previousProjectId);
            if (images == null)
            {
                return BadRequest(new { Message = $"Image with Pervious Project {previousProjectId} is not found." });
            }

            var dto = mapper.Map<List<PortofolioProjectImageDTO>>(images);
            return Ok(dto);
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PortofolioProjectImageDTO>> UploadImage([FromForm] UploadImageRequest request)
        {
            if (request.ImageFile == null || request.ImageFile.Length == 0)
                return BadRequest(new { Message = "No image uploaded" });

            var project = await projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                return BadRequest(new { Message = $"Project with ID {request.ProjectId} not found." });

            }
               

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); // Ensure folder exists

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            var image = new PortofolioProjectImage
            {
                Image =  fileName,
                //PreviousProjectId = request.ProjectId
            };

            await context.AddAsync(new PortofolioProjectImageDTO
            {
                Image = image.Image,
                PreviousProjectId = request.ProjectId
            });

            return Ok(image);
        }


        // DELETE: api/PortofolioProjectImage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortofolioProjectImage(int id)
        {
            var portofolioProjectImageDTO = await context.DeleteAsync(id);
            if (!portofolioProjectImageDTO)
            {
                return BadRequest(new { Message = $"Unable to find or delete image with ID {id}" });
            }


            return Ok(new { Message = "Image marked as deleted successfully" });
        }

    }
}
