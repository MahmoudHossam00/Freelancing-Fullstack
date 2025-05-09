using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class AddingFunds: Payment
	{
		[ForeignKey("Freelancer")]
		public string? FreelancerId { set; get; }
		public Freelancer Freelancer { set; get; }
		[ForeignKey("Client")]
		public string? ClientId { set; get; }
		public Client Client { set; get; }
	}
}
