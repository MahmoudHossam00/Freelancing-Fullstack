using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs
{
    public class FreelancerLanguageDTO
    {
        public int id { set; get; }
        public Language Language { get; set; }
        public bool IsDeleted { get; set; } = false;        
        public string freelancerName { set; get; }
    }
}
