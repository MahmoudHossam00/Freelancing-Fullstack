using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Freelancing.Models
{
	public class PortofolioProject
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; } // upwork m3mlhash
        public bool IsDeleted { get; set; } = false;
        public virtual List<PortofolioProjectImage> Images { get; set; } = new List<PortofolioProjectImage>();


		[ForeignKey("Freelancer")]
		public string FreelancerId { get; set; }
        [JsonIgnore]
        public virtual Freelancer? Freelancer { get; set; }


	}
}
