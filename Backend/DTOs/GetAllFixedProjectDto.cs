namespace Freelancing.DTOs
{
    public class GetAllFixedProjectDto
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; } //price for the whole project
        public string Description { get; set; }
        public string ProjectType { get; set; } = "Fixed";
        public int PostedFrom { get; set; }


        public Currency Currency { get; set; }

        public int ExpectedDuration { get; set; }

        public int SubcategoryID { get; set; }

        public ExperienceLevel ExperienceLevel { get; set; }
        public int ProposalsCount { get; set; }
        public List<string> ProjectSkills { get; set; } = new List<string>();
        public List<MilestoneDto> Milestones { get; set; } = new List<MilestoneDto>();

        public string ClientId { get; set; }

        public double ClientRating { get; set; }

        public int ClientTotalNumberOfReviews { get; set; }

        public bool ClientIsverified { get; set; }

        public string ClientCountry { get; set; }
        public string ClientCity { get; set; }





        public string ClinetAccCreationDate { get; set; }
        public string FreelancersubscriptionPlan { get; set; }
        public int FreelancerTotalNumber { get; set; }
        public int? FreelancerRemainingNumberOfBids { get; set; }
        public List<int> ClientOtherProjectsIdsNotAssigned { get; set; }
        public int ClientProjectsTotalCount { get; set; }

        public string? FreelancerId { get; set; }


    }
}
