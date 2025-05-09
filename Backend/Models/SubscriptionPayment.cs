namespace Freelancing.Models
{
	public class SubscriptionPayment : Payment
	{
		public virtual UserSubscriptionPlanPayment payments { get; set; }
	}
}
