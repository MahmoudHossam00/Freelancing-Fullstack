using Freelancing.DTOs.ProposalDTOS;

namespace Freelancing.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public int ExpectedDuration { get; set; }
        public string ClientId { get; set; }
        public string? FreelancerId { get; set; }
		public projectType projectType { set; get; }
		public projectStatus Status { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public List<MilestoneDto> milestones { set; get; }

		public bool IsDeleted { get; set; }
    }
}
