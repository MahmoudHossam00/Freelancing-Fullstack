using Freelancing.DTOs;

namespace Freelancing.IRepositoryService
{
	public interface ICityService
	{
		public List<City> GetAll();
		public City GetById(int id);
		public void Create(CityViewModel vm);
		public void Update(CityViewModel vm);
		public void Delete(int id);
	}
}
