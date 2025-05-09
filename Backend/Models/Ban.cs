using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class Ban
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime BanDate { get; set; }
		public DateTime BanEndDate { get; set; }
		[ForeignKey("BannedUser")]
		public string BannedUserId { get; set; } 
		public virtual AppUser BannedUser { get; set; } 
	}
}
