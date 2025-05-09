using System.ComponentModel.DataAnnotations;

namespace Freelancing.DTOs.AuthDTOs
{
	public class LoginDTO
	{
		public string Usernameoremail { set; get; }
		public string loginPassword { set; get; }
	}
}
