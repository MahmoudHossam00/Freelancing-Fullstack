using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class NotificationProfile :Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<UpdateNotificationDto, Notification>();
            CreateMap<Notification, UpdateNotificationDto>();
        }
    }
}
