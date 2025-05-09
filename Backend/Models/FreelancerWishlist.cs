namespace Freelancing.Models
{
	public class FreelancerWishlist
	{
		public int Id { get; set; }
		public string FreelancerId { get; set; }
		public Freelancer Freelancer { get; set; }
		public int ProjectId { set; get; }
		public Project Project { get; set; }
	}
}
