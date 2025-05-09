using Freelancing.Attributes;

namespace Freelancing.DTOs
{
	public class CreatePortfolioProjectDTO
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { set; get; }
		[ImageExtension]
		public virtual List<IFormFile>? Images { get; set; } 
	}
}
