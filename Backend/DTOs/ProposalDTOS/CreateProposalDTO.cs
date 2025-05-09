using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.ProposalDTOS
{
	public class CreateProposalDTO
	{
		public string CoverLetter { get; set; }


		[NotMapped]
		public decimal? Price => type==projectType.bidding? this.suggestedMilestones.Sum(x => x.Amount):null;


		public int ProjectId { get; set; }


		[NotMapped]
		public int SuggestedDuration => this.suggestedMilestones.Sum(x => x.Duration); //in days

		public List<CreateProposalSuggestedMilestoneDTO> suggestedMilestones { get; set; }

		public projectType type {set;get;}
	}
	public enum projectType
	{
		bidding,
		fixedprice
	}
}
