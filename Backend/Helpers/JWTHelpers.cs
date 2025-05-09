using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PharmaTechBE.Helpers
{
	public static class JWTHelpers
	{
		
		public static string CreateRefreshToken()
		{
			var refreshtoken = new byte[32];
			var rng = RandomNumberGenerator.Create();
			rng.GetBytes(refreshtoken);
			return Convert.ToBase64String(refreshtoken);
		}
	}
}
