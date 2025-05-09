namespace Freelancing.DTOs.AuthDTOs
{
	public class ViewClientDTO
	{
		public string Id { set; get; }
		public DateOnly AccountCreationDate { set; get; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public string UserName { set; get; }
		public string Country { get; set; }
		public string City { get; set; }
		public string Email { set; get; }
		public string? ProfilePicture { get; set; }
		public bool IsVerified { get; set; }
		public bool PaymentVerified { get; set; } 

	}
}
