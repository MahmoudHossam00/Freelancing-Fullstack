namespace Freelancing.Models
{
	public class ClientProposalPayment:Payment
	{
		public int ProposalId { set; get; }
        public Proposal Proposal { set; get; }
    }
}
