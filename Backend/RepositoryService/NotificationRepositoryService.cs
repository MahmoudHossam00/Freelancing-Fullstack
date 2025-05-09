using Freelancing.DTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Freelancing.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class NotificationRepositoryService : INotificationRepositoryService
    {

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _NotifhubContext;

		public NotificationRepositoryService(ApplicationDbContext context,IHubContext<NotificationHub> NotifhubContext)
        {

			_context = context;
            _NotifhubContext = NotifhubContext;

		}
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            notification.isRead = false;
            _context.Notifications.Add(notification);
           await _context.SaveChangesAsync();
			await _NotifhubContext.Clients.Users(notification.UserId).SendAsync("ReceiveNotification",
					new NotificationDto()
					{
						Id = 1,
						IsRead = false,
						Message = notification.Message,
						UserId = notification.UserId
					});
			return notification;
        }
        public async Task DeleteNotificationAsync(int id)
        {
            var notification = _context.Notifications.Find(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
                return await _context.Notifications.Include(n => n.User)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Id)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            return   await _context.Notifications.Include(n => n.User)
                    .Where(n => n.UserId == userId && n.isRead == false)
                    .OrderByDescending(n => n.Id)
                    .ToListAsync();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var unreadNotifications = _context.Notifications
                .Where(n => n.UserId == userId && n.isRead == false).ToListAsync();
            foreach (var notification in unreadNotifications.Result)
            {
                notification.isRead = true;
            }
                await _context.SaveChangesAsync();

        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if(notification != null)
            {
                notification.isRead = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
