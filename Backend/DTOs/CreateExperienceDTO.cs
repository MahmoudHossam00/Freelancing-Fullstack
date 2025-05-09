namespace Freelancing.DTOs
{
    public class CreateExperienceDTO
    {
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
