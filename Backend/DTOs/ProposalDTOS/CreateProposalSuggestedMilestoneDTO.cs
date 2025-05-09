using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.ProposalDTOS
{
	public class CreateProposalSuggestedMilestoneDTO
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public int Duration { get; set; }
	}
}
