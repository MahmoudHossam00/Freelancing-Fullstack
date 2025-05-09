using Freelancing.Models;

namespace Freelancing.DTOs
{
    public class PortofolioProjectDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FreelancerId { get; set; }
        public virtual List<PortofolioProjectImage> Images { get; set; } = new List<PortofolioProjectImage>();
    }
}
