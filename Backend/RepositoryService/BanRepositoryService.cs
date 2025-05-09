using Freelancing.DTOs.AuthDTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class BanRepositoryService : IBanRepositoryService
    {
        private readonly ApplicationDbContext _context;
        public BanRepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Ban?> GetBanByIdAsync(int id)
        {
            return await _context.Bans
                .Include(b => b.BannedUser)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<Ban>> GetBansByUserIdAsync(string userId)
        {
            return await _context.Bans.Include(b => b.BannedUser)
                .Where(b => b.BannedUserId == userId)
                .OrderBy(b => b.BanDate)
                .ToListAsync();
        }
        public Task<List<Ban>> GetActiveBansByUserIdAsync(string userId)
        {
            var currentDate = DateTime.UtcNow;
            return _context.Bans.Include(b => b.BannedUser)
                .Where(b => b.BannedUserId == userId &&
                       b.BanDate <= currentDate &&
                       b.BanEndDate >= currentDate)
                .OrderBy(b => b.BanDate)
                .ToListAsync();
        }
        public async Task<Ban> CreateBanAsync(Ban ban)
        {
            ban.BanDate = DateTime.UtcNow;
            _context.Bans.Add(ban);
            await _context.SaveChangesAsync();
            return ban;

        }
        public async Task UpdateBanAsync(Ban ban)
        {
            //var ban=GetBanByIdAsync(ban.Id);
            _context.Bans.Update(ban);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBanAsync(int id)
        {
            var ban = _context.Bans.Find(id);
            if (ban != null)
            {
                _context.Bans.Remove(ban);
                await _context.SaveChangesAsync();
            }

        }
        public async Task<bool> IsUserBannedAsync(string userId)
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Bans.AnyAsync(b => b.BannedUserId == userId &&
                b.BanDate <= currentDate &&
                b.BanEndDate >= currentDate);

        }

        public async Task<List<Ban>> GetBannedUsersAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Bans.Include(b => b.BannedUser)
                //.Where(b => b.BanDate <= currentDate &&
                //       b.BanEndDate >= currentDate)
                //.Select(b => b.BannedUser)
                .ToListAsync();
        }

      
    }
}
