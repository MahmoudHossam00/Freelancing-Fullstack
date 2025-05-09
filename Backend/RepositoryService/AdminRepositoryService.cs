using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
	public class AdminRepositoryService(ApplicationDbContext _context):IAdminRepositoryService
	{
		public async Task<bool> AddAdmin(Admin Admin)
		{
			await _context.Admins.AddAsync(Admin);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> AdminExists(string id)
		{
			return await _context.Admins.AnyAsync(c => c.Id == id);
		}

		public async Task<bool> DeleteAdmin(string id)
		{
			var Admin = await _context.Admins.FirstOrDefaultAsync(c => c.Id == id);
			if (Admin == null)
			{
				return false;
			}
			Admin.isDeleted = true;
			_context.Admins.Update(Admin);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<Admin>> GetAllAdmins()
		{
			return await _context.Admins.ToListAsync();
		}

		public async Task<Admin> GetAdminById(string id)
		{
			return await _context.Admins.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<bool> UpdateAdmin(Admin Admin)
		{
			var admin = await GetAdminById(Admin.Id);
			if (admin == null)
			{
				return false;
			}
			else
			{
				_context.Admins.Update(Admin);
				return await _context.SaveChangesAsync() > 0;
			}
		}
	}
}
