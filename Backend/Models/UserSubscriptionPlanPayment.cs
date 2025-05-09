using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class UserSubscriptionPlanPayment
	{
		public int Id { get; set; }
		public bool isDeleted { get; set; } = false;



		[ForeignKey("SubscriptionPlan")]
		public int SubscriptionPlanId { get; set; }
		public virtual SubscriptionPlan SubscriptionPlan { get; set; } //navigation property



		[ForeignKey("User")]
		public string UserId { get; set; }
		public virtual AppUser User { get; set; } //navigation property



		[ForeignKey("SubscriptionPayment")]
		public int SubscriptionPaymentId { get; set; }
		public SubscriptionPayment SubscriptionPayment { get; set; } //navigation property
	}
}
