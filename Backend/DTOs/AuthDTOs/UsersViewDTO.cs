namespace Freelancing.DTOs.AuthDTOs
{
	public class UsersViewDTO
	{
		public string Id { get; set; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public string? Title { set; get; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string? ProfilePicture { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string? NationalId { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string Description { get; set; }
		public Accountrole role { get; set; }
		public bool IsVerified { get; set; }
		public DateOnly AccountCreationDate { get; set; }
		public bool emailConfirmed { get; set; }
		public decimal Balance { set; get; }
		public bool? isAvailable { get; set; }
		public bool? PaymentVerified { get; set; }
	}
	
public enum Accountrole
{
	Freelancer,
	Client,
	Admin
}
}
