using Freelancing.DTOs;
using Freelancing.DTOs.AuthDTOs;
using Freelancing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CityController : ControllerBase
	{
		private readonly ICityService _cityService;
		private readonly ICountryService _countryservice;
        private readonly ApplicationDbContext _context;

        public CityController(ICityService cityService, ICountryService countryService ,ApplicationDbContext context)
		{
			_cityService = cityService;
			_countryservice = countryService;
            this._context = context;
        }
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_cityService.GetAll().Select(c=>new {c.Name,countryname=c.Country.Name,c.CountryId,c.Id}));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var city = _cityService.GetById(id);
			if (city == null)
			{
				return BadRequest(new { Message = "City Not Found" });
			}

			return Ok(new { city.Name, countryname = city.Country.Name, city.Id });
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult Create(CityViewModel vm)
		{
			if(vm.Id !=null && vm.Id!=0)
			{
				return BadRequest(new { Message = "cannot be created dont insert an id" });
			}
			if (_countryservice.GetById(vm.CountryId) == null)
			{
				return BadRequest(new { Message = "Invalid Country" });
			}
			_cityService.Create(vm);
			return Ok(new { Message = "Created" });
		}

        //// GET: Countries/Edit/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, [FromBody] CityViewModel vm)
        {
            if (id != vm.Id)
            {
                return BadRequest(new { Message = "ID mismatch" });
            }

            var city = _cityService.GetById(id);
            if (city == null)
            {
                return NotFound(new { Message = "City not found" });
            }

            var country = _context.Countries.Find(vm.CountryId);
            if (country == null)
            {
                return BadRequest(new { Message = "Invalid Country" });
            }

            try
            {
                _cityService.Update(vm);
                return Ok(new { Message = "City updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the city", Details = ex.Message });
            }
        }


        //// POST: Countries/Delete/5
        [HttpDelete]
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int id)
		{
			_cityService.Delete(id);

			return Ok(new { Message = "deleted" });
		}

	}
}
