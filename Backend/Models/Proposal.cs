using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelancing.Models
{
	public class Proposal
	{
		public int Id { get; set; }
		//public string title { get; set; }
		public string CoverLetter { get; set; }
		public decimal? Price { get; set; } // nullable for fixedpriceprojects


		[ForeignKey("Freelancer")]
		public string FreelancerId { get; set; }
		public virtual Freelancer Freelancer { get; set; }


		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }

		public int SuggestedDuration { get; set; } //in days
		public bool IsDeleted { get; set; } = false;
		public List<SuggestedMilestone> suggestedMilestones { get; set; }

	}
}
