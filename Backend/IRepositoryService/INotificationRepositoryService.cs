using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface INotificationRepositoryService
    {
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<List<Notification>> GetNotificationsByUserIdAsync(string userId);
        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(int id);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(string userId);
    }
}
