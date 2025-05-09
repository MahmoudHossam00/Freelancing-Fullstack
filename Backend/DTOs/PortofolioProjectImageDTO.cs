using System.Text.Json.Serialization;

namespace Freelancing.DTOs
{
    public class PortofolioProjectImageDTO
    {
        public int? Id { get; set; }
        public string Image { get; set; }
        [JsonIgnore]
        public int PreviousProjectId { get; set; }
    }
}
