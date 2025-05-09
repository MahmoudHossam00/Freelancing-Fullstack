namespace Freelancing.Models
{
	public class BiddingProject:Project
	{
		public int minimumPrice { get; set; }
		public int maximumprice { get; set; }
		public int BidCurrentPrice { get; set; }
		public DateTime BiddingStartDate { get; set; } 
		public DateTime BiddingEndDate { get; set; } 
	}
}
