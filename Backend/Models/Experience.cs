using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class Experience
	{
		public int Id { get; set; }
		public string JobTitle { get; set; } 
		public string Company { get; set; } 
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; } 
		public string Location { get; set; } 
		public string Description { get; set; }

		public bool isDeleted { get; set; } = false;
		[ForeignKey("Freelancer")]
		public string FreelancerId { get; set; }
		public Freelancer Freelancer { get; set; } 

	}
}
