using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
    public class SuggestedMilestone
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }

        [ForeignKey("Proposal")]
        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }
    }
}
