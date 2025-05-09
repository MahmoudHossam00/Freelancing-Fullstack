namespace Freelancing.IRepositoryService
{
	public interface IAdminRepositoryService
	{
		Task<bool> AddAdmin(Admin Admin);
		Task<bool> UpdateAdmin(Admin Admin);
		Task<bool> DeleteAdmin(string id);
		Task<Admin> GetAdminById(string id);
		Task<IEnumerable<Admin>> GetAllAdmins();
		Task<bool> AdminExists(string id);
	}
}
