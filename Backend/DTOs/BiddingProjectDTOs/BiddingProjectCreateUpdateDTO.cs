namespace Freelancing.DTOs.BiddingProjectDTOs
{
    public class BiddingProjectCreateUpdateDTO
    {
        
        public string Title { get; set; }
        public string Description { get; set; }

        public int currency { set; get; }

        public int minimumPrice { get; set; }
        public int maximumprice { get; set; }

        public DateTime BiddingStartDate { get; set; }
        public DateTime BiddingEndDate { get; set; }

        //  public string ExperienceLevel { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }



        public int ExpectedDuration { get; set; }

        public List<int> ProjectSkillsIds { get; set; }
        public int SubcategoryId { get; set; }
    }
        //public List<string> ProjectSkills { get; set; }
        //public string SubcategoryName { get; set; }
        //public string ClientId { get; set; }
}
