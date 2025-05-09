using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.ProposalDTOS
{
	public class ProposalViewDTO
	{
		public int Id { get; set; }
		public string title { get; set; }	

		public string CoverLetter { get; set; }
		public decimal? Price { get; set; } // nullable for fixedpriceprojects
		public int ProjectId { set; get; }
		public string FreelancerName { get; set; }
		public string FreelancerProfilePicture { set; get; }
		public List<string> Freelancerskills { set; get; }
		public List<string> FreelancerLanguages { set; get; }
		public proposalstatus proposalStatus { set; get; }
		public projectType projecttype { set; get; }
		public bool IsVerified { get; set; }
		public string Country { get; set; }

		public int SuggestedDuration { get; set; } //in days

		public List<MilestoneViewDTO> suggestedMilestones { get; set; }
		public Rank rank{ set; get; }
	}
	public enum proposalstatus
	{
		Pending,
		Accepted,
		Rejected
	}
}
