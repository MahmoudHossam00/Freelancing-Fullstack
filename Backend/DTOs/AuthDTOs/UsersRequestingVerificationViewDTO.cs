namespace Freelancing.DTOs.AuthDTOs
{
	public class UsersRequestingVerificationViewDTO
	{
		public string Id { set; get; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string City { set; get; }
		public string Country { set; get; }
		public string? ProfilePicture { get; set; }
		public string? NationalId { get; set; }
	}
}
