namespace Freelancing.DTOs.AuthDTOs
{
	public class VerificationDecision
	{
		public bool isAccepted { set; get; }
		public string userId { set; get; }
		public string? Reason { set; get; }
	}
}
