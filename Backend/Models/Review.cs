using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class Review
	{
		public int id { set; get; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		public DateTime Date { get; set; }


		[ForeignKey("Reviewee")]
		public string RevieweeId { set; get; }
		public AppUser Reviewee { set; get; }



		[ForeignKey("Reviewer")]
		public string ReviewerId { set; get; }
		public AppUser Reviewer { set; get; }



		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public Project Project { get; set; }

        public string? Sentiment { get; set; }
        public double? SentimentScore { get; set; }


    }
}
