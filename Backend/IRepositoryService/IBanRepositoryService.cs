using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IBanRepositoryService
    {
        Task<Ban> GetBanByIdAsync(int id);
        Task<List<Ban>> GetBansByUserIdAsync(string userId);
        Task<List<Ban>> GetActiveBansByUserIdAsync(string userId);
        Task<Ban> CreateBanAsync(Ban ban);
        Task UpdateBanAsync(Ban ban);
        Task DeleteBanAsync(int id);

        Task<List<Ban>>GetBannedUsersAsync();
        Task<bool> IsUserBannedAsync(string userId);
    }
}
