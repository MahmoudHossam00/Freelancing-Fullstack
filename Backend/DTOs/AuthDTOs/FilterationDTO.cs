using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs.AuthDTOs
{
	public class FilterationDTO
	{
		public string? name { get; set; } = default;
		public DateOnly? AccountCreationDate { get; set; } = default;
		public List<int>? CountryIDs { get; set; } = null;
		public bool? IsVerified { get; set; } = false;
		public bool? Paymentverified { get; set; } = false;
		public int pagesize { get; set; } = 3;
		public int pageNum { get; set; } = 0;

		[NotMapped]
		public int numofpages { get; set; }
	}
}
