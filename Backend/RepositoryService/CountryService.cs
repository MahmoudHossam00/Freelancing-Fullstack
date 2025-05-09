using Freelancing.DTOs.AuthDTOs;

namespace Freelancing.RepositoryService
{
	public class CountryService(ApplicationDbContext context):ICountryService
	{
		public List<Country> GetAll()
		{
			return context.Countries.Include(c => c.Cities).Where(c => !c.isDeleted).ToList();
		}

		public Country GetById(int id)
		{
			return context.Countries.Include(c => c.Cities).SingleOrDefault(c => c.Id == id);
		}
		public void Create(CountryViewModel vm)
		{
			Country cntry = new()
			{
				Name = vm.Name,
			};

			context.Countries.Add(cntry);
			context.SaveChanges();
		}

		public void Update(CountryViewModel vm)
		{
			var cntry = context.Countries.FirstOrDefault(c => c.Id == vm.Id);
			cntry.Name = vm.Name;
	
			//context.Countries.Update(cntry);
			context.SaveChanges();
		}
		public void Delete(int id)
		{
			var cntry = context.Countries.FirstOrDefault(c => c.Id == id);
			cntry.isDeleted = true;
			context.Countries.Update(cntry);
			context.SaveChanges();
		}
	}
}
