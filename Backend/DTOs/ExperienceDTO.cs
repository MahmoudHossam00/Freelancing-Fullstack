namespace Freelancing.DTOs
{
    public class ExperienceDTO
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool isDeleted { get; set; }
        public string FreelancerName { get; set; }

    }
}
