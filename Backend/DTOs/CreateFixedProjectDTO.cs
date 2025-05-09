using System.ComponentModel.DataAnnotations;

namespace Freelancing.DTOs
{
    public class CreateFixedProjectDTO
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } //price for the whole project

        public Currency Currency { get; set; }

        public int ExpectedDuration { get; set; }

      //  public string SubcategoryName { get; set; }
        public int SubcategoryID { get; set; }



        public ExperienceLevel ExperienceLevel { get; set; }
        public int ProposalsCount { get; set; }
        public List<int> ProjectSkills { get; set; } = new List<int>();
        public List<MilestoneDto> Milestones { get; set; } = new List<MilestoneDto>();

    }



    public class MilestoneDto
    {

        
        public string Title { get; set; }

        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public MilestoneStatus Status { get; set; }
    }


}
