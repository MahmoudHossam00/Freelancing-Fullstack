namespace Freelancing.DTOs.BiddingProjectDTOs
{
    public class BiddingProjectDTO //create & update
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubcategoryId { get; set; }
        public int ExpectedDuration { get; set; }
        public int minimumPrice { get; set; }
        public int maximumprice { get; set; }
        public Currency currency { set; get; }
        public ExperienceLevel experienceLevel { get; set; }
        public List<string> ProjectSkills { get; set; } = new List<string>();
    }
}
