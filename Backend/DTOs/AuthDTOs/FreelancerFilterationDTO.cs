using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.AuthDTOs
{
	public class FreelancerFilterationDTO : FilterationDTO
	{
		public bool? isAvailable { get; set; } = false;
		public List<Language>? Languages { get; set; } = new List<Language>();
		public List<Rank>? ranks { get; set; } = new List<Rank>();
	}
}
