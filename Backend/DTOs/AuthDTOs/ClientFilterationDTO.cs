using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.AuthDTOs
{
	public class ClientFilterationDTO:FilterationDTO
	{
		public bool? Paymentverified { set; get; }
		public List<Rank>? ranks { get; set; } = new List<Rank>();

	}
}
