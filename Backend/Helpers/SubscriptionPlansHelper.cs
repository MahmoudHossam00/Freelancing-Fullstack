namespace Freelancing.Helpers
{
	public static class SubscriptionPlansHelper
	{
	 public static List<SubscriptionPlan> SubscriptionPlans = new List<SubscriptionPlan>()
			{ new SubscriptionPlan()
			{
				Id=1,
				name="Starter",
				Description="Basic access, limited bids/applications.",
				Price=0,
				TotalNumber=6,
				DurationInDays=30
			},
			new SubscriptionPlan()
			{
				Id=2,
				name="Pro Freelancer",
				Description="More bids, priority support.",
				Price=100,
				TotalNumber=30,
				DurationInDays=60
			},
			new SubscriptionPlan()
			{
				Id=3,
				name="Elite",
				Description="Maximum exposure, profile boosts.",
				Price=200,
				TotalNumber=60,
				DurationInDays=90
			}

			};
	}
}
