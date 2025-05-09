using AutoMapper;
using Freelancing.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepositoryService notificationService;
        private readonly IMapper mapper;

        public NotificationsController(INotificationRepositoryService notificationService, IMapper mapper)
        {
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto createNotificationDto)
        {

            var notification = mapper.Map<Notification>(createNotificationDto);
            var CreatedNotification = await notificationService.CreateNotificationAsync(notification);
            var notificationDto = mapper.Map<NotificationDto>(CreatedNotification);


            return CreatedAtAction(nameof(GetNotificationById), new { id = notificationDto.Id }, notificationDto);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationById(int id)
        {
            var notification = await notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return BadRequest(new { Message = "No Notification Found" });
            }
            var notificationDto = mapper.Map<NotificationDto>(notification);
            return Ok(notificationDto);

        }

        [HttpGet("user")]
        public async Task<ActionResult<List<NotificationDto>>> GetNotificationsByUserId()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var notifications = await notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(mapper.Map<IEnumerable<NotificationDto>>(notifications));
        }
        [HttpGet("unread")]
        public async Task<ActionResult<List<NotificationDto>>> GetUnreadNotifications()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var notifications = await notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(mapper.Map<IEnumerable<NotificationDto>>(notifications));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, UpdateNotificationDto updateNotificationDto)
        {
            if (id != updateNotificationDto.Id)
            {
                return BadRequest(new { Message = "Not Found" });
            }

            var notification = await notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return BadRequest(new { Message = "No Notification Found" });
            }

            mapper.Map(updateNotificationDto, notification);
            await notificationService.UpdateNotificationAsync(notification);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return BadRequest(new { Message = "No Notification Found" });
            }
            await notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }

        [HttpPut("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return BadRequest(new { Message = "No Notification Found" });
            }
            await notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            await notificationService.MarkAllAsReadAsync(userId);
            return NoContent();
        }
    }

}
