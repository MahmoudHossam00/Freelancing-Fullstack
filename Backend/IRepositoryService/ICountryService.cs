using Freelancing.DTOs.AuthDTOs;

namespace Freelancing.IRepositoryService
{
	public interface ICountryService
	{
		public List<Country> GetAll();
		public Country GetById(int id);
		public void Create(CountryViewModel vm);
		public void Update(CountryViewModel vm);
		public void Delete(int id);
	}
}
