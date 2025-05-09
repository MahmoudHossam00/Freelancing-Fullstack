namespace Freelancing.DTOs
{
    public class CreateFreelancerLanguageDTO
    {
        public Language Language { get; set; }
        public bool IsDeleted { get; set; } = false;        
    }
}
