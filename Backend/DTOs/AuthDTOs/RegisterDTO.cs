using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Freelancing.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Freelancing.DTOs.AuthDTOs
{
	public class RegisterDTO
	{
		public string firstname { get; set; }
		public string lastname { get; set; }
		public int CityId { set; get; }
		public string UserName { set; get; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[myAge]
		public DateOnly DateOfBirth { set; get; }

		public string Email { set; get; }

		[Phone]
		[MaxLength(11)]
		[MinLength(11)]
		public string PhoneNumber { set; get; }
		[DataType(DataType.Password)]
		
		public string Password { set; get; }
		[Compare("Password")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { set; get; }

		[ImageExtension]
		[NotMapped]
		public IFormFile? ProfilePicture { set; get; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public userRole Role { set; get; }

	}
	public enum userRole
	{
		Freelancer,
		Client
	}
}
