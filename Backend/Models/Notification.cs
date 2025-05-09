namespace Freelancing.Models
{
	public class Notification
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public bool isRead { get; set; } = false;
	}
}
