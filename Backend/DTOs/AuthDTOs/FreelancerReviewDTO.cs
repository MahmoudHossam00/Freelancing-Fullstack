namespace Freelancing.DTOs.AuthDTOs
{
	public class FreelancerReviewDTO
	{
		public int Id { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		public string ReviewerId { get; set; }
		public string ReviewerName { get; set; }
	}
}
