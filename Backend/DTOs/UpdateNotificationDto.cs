namespace Freelancing.DTOs
{
    public class UpdateNotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
