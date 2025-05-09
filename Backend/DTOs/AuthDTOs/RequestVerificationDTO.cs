namespace Freelancing.DTOs.AuthDTOs
{
	public class RequestVerificationDTO
	{
		public string fullName { get; set; }
		public string nationalId { set; get; }
		public IFormFile IdPicture { set; get; }
	}
}
