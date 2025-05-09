namespace Freelancing.DTOs
{
    public class GetReviewByRevieweeIdDto
    {
        public int id { set; get; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string ReviewerName { set; get; }
        public string? ReviewerId { set; get; }
        public int ProjectId { get; set; }
        public string ProjectTitle { set; get; }
        public string ProjectType { get; set; }
    }
}
