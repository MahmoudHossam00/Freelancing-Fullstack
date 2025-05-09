using Freelancing.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
	public class CityService(ApplicationDbContext context) : ICityService
	{
		public List<City> GetAll()
		{
			return context.Cities.Include(c => c.Country).Where(c => !c.isDeleted).ToList();
		}

		public City GetById(int id)
		{
			return context.Cities.Include(c => c.Country).SingleOrDefault(c => c.Id == id);
		}

		public void Create(CityViewModel vm)
		{
			City city = new City()
			{
				Name = vm.Name,
				CountryId = vm.CountryId
			};
			context.Cities.Add(city);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			var cty = context.Cities.SingleOrDefault(c => c.Id == id);
			cty.isDeleted = true;
			context.Cities.Update(cty);
			context.SaveChanges();
		}


		public void Update(CityViewModel vm)
		{
			var city = context.Cities.SingleOrDefault(c => c.Id == vm.Id);
			if (city == null)
			{
				throw new Exception("City not found");
			}

			city.Name = vm.Name;
			city.CountryId = vm.CountryId;
			context.SaveChanges();
		}
	}
}
