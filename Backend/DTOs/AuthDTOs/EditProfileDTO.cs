using Freelancing.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Freelancing.DTOs.AuthDTOs
{
	public class EditProfileDTO
	{
		public string firstname { get; set; }
		public string lastname { get; set; }
		public int CityId { set; get; }
		public string? Title { set; get; }
		public string UserName { set; get; }
		[MaxLength(500)]
		public string? Description { set; get; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[myAge]
		public DateOnly DateOfBirth { set; get; }


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
		public IFormFile? ProfilePicture { set; get; }
	}
}
