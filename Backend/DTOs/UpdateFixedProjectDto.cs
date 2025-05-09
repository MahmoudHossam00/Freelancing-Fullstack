namespace Freelancing.DTOs
{
    public class UpdateFixedProjectDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }
        public int ExpectedDuration { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public List<int> ProjectSkills { get; set; } = new List<int>();
        public string SubcategoryName { get; set; }
        public int ProposalsCount { get; set; }
        public List<MilestoneDto> Milestones { get; set; } = new List<MilestoneDto>();




    }
}
