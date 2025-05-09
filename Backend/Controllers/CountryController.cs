using Freelancing.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Freelancing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController(ICountryService _countryService) : ControllerBase
	{
		// GET: Countries
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_countryService.GetAll());
		}

		// GET: Countries/Details/5
		[HttpGet("/{id}")]
		public IActionResult GetById(int id)
		{
			var country = _countryService.GetById(id);
			if (country == null)
			{
				return BadRequest(new { Message = "No Country Found" });
			}

			return Ok(country);
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public IActionResult Create(CountryViewModel vm)
		{
			if (vm.Id != null && vm.Id != 0)
			{
				return BadRequest(new { Message = "cannot be created dont insert an id" });
			}
			_countryService.Create(vm);
			return Ok(new { Message = "Created" });
		}

		// GET: Countries/Edit/5
		[HttpPut]
		[Authorize(Roles = "Admin")]
		public IActionResult Edit(CountryViewModel vm)
		{

			if (vm.Id is null)
			{
				return BadRequest(new { Message = "Not Found" });
			}
			var country = _countryService.GetById((int)vm.Id);

			CountryViewModel newvm = new()
			{
				Id = vm.Id,
				Name = country.Name,
			};
			if (country == null)
			{
				return BadRequest(new { Message = "No Country Found" });
			}
			_countryService.Update(newvm);
			return Ok(new { Message = "Updated" });
		}



		// POST: Countries/Delete/5
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int id)
		{
			_countryService.Delete(id);

			return Ok(new { Message = "deleted" });
		}

	}
}
