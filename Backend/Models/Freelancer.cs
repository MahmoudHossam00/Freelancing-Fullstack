using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class Freelancer : AppUser
	{
		public List<Certificate> Certificates { get; set; }
		public List<UserSkill> UserSkills { get; set; }
		public bool isAvailable { get; set; } = true;
		public decimal Balance { get; set; } = 0;
		public string? StripeAccountId {set;get;}
		public string? Description { set; get; } = string.Empty;
		public Education Education { get; set; }
		public List<Experience> Experiences { get; set; }
		public List<FreelancerLanguage> Languages { get; set; }
		public List<PortofolioProject> Portofolio { get; set; }

		//public bool IsDeleted { get; set; } = false;


		[ForeignKey("subscriptionPlan")]
		public int? subscriptionPlanId { get; set; }
		public virtual SubscriptionPlan? subscriptionPlan { get; set; }
		public virtual List<Withdrawal>  Withdrawals {set;get;}
		public int? RemainingNumberOfBids { get; set; } = 6;

		public virtual List<NonRecommendedUserSkill> NonRecommendedUserSkills { get; set; }

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
	public enum Language
	{
		English,
		Spanish,
		French,
		German,
		Italian,
		Russian,
		Chinese,
		Japanese,
		Korean,
		Hindi
	}
	public enum FreelancerStatus
	{
		Available,
		Unavailable
	}
}
