using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.ProposalDTOS
{
	public class CreateFixedPriceProposalDTO
	{
		public string CoverLetter { get; set; }
		public int ProjectId { get; set; }
		public int SuggestedDuration { get; set; } //in days
		public List<CreateProposalSuggestedMilestoneDTO> suggestedMilestones { get; set; }
	}
}
