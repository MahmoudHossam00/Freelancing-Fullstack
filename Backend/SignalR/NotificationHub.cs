using Freelancing.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Freelancing.SignalR
{
	public class NotificationHub(ApplicationDbContext _context):Hub
	{
		public async Task Notify(NotificationDto notificationDto)
		{
			

			if (notificationDto == null || string.IsNullOrEmpty(notificationDto.UserId))
			{
				throw new HubException("Invalid message data");
			}

			await Clients.Users(notificationDto.UserId)
				.SendAsync("ReceiveNotification", notificationDto);
		}

	}
}
