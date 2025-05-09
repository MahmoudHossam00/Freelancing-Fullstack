using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.ProposalDTOS
{
	public class EditProposalDTO
	{
		//public int Id { set; get; }
		public string CoverLetter { get; set; }
		[NotMapped]
		public decimal? Price => type == projectType.bidding ? this.suggestedMilestones.Sum(x => x.Amount) : null;
		[NotMapped]
		public int SuggestedDuration => this.suggestedMilestones.Sum(x => x.Duration); //in days
		public List<UpdateProposalSuggestedMilestoneDTO> suggestedMilestones { get; set; }
		public projectType type { set; get; }
	}
}
