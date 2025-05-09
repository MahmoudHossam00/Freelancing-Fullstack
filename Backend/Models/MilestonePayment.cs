namespace Freelancing.Models
{
	public class MilestonePayment:Payment
	{
		public int MilestoneId { get; set; }
		public virtual Milestone Milestone { get; set; }
    }
}
