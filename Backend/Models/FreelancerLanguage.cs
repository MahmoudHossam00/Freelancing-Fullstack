using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class FreelancerLanguage
	{
		public int id { set; get; }
		public Language Language { get; set; }
		public bool IsDeleted { get; set; }=false;

		[ForeignKey("freelancer")]
		public string freelancerId { set; get; }
		public Freelancer freelancer { set; get; }
	}
}
