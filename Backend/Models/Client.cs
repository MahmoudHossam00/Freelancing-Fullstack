using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class Client : AppUser
	{
		public bool PaymentVerified { get; set; } = false;


        [ForeignKey("subscriptionPlan")]
        public int? subscriptionPlanId { get; set; }
        public virtual SubscriptionPlan? subscriptionPlan { get; set; }
		public int? RemainingNumberOfProjects { get; set; } = 6;

        public decimal Balance { get; set; } = 0;
        public string? StripeAccountId { set; get; }


        [NotMapped]
		public Rank Rank
		{
			get
			{
				int count = Reviewed.Count;
				if (count == 0)
					return Rank.Veteran;

				double average = Reviewed.Average(r => r.Rating);

				if (count < 5 && average >= 3.5)
					return Rank.RisingStar;
				else if (count < 15 && average >= 4)
					return Rank.Established;
				else if (count < 30 && average >= 4.2)
					return Rank.Pro;
				else if (count >= 30 && average >= 4.5)
					return Rank.Elite;

				return Rank.RisingStar;
			}
		}

	}
    public enum Rank
    {
	
		Veteran,       
		RisingStar,    
		Established,   
		Pro,           
		Elite          
	
	}
}
