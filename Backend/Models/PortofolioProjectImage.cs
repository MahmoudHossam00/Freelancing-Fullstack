using System.Text.Json.Serialization;

namespace Freelancing.Models
{
    public class PortofolioProjectImage
    {
		public int Id { get; set; }
		public string Image { get; set; }
		public int PreviousProjectId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        public PortofolioProject PreviousProject { get; set; }

        //public int? PortofolioProjectId { get; set; }
        //public PortofolioProject PortofolioProject { get; set; }

// If the image is for a previous project → set PreviousProjectId

//If the image is for a portfolio project → set PortofolioProjectId
    }
}
