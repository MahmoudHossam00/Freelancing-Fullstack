namespace Freelancing.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
    }
}
