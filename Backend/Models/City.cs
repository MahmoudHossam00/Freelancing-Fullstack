using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Freelancing.Models
{
	public class City
	{
		[Key]
		public int Id { set; get; }
		[MinLength(3)]
		public string Name { set; get; }
		[ForeignKey("Country")]
		public int CountryId { set; get; }
		public virtual Country Country { set; get; }
		public virtual List<AppUser> Users { set; get; }
		[DefaultValue(false)]
		public bool isDeleted { set; get; } = false;
	}
}
