namespace Freelancing.DTOs.ProposalDTOS
{
	public class MilestoneViewDTO
	{
		public int id { set; get; }
		public string title { set; get; }
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public int Duration { get; set; }
	}
}
