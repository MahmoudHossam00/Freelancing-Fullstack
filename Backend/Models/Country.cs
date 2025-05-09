using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Freelancing.Models
{
	public class Country
	{
		[Key]
		public int Id { set; get; }
		[Required]
		[MinLength(3)]
		public string Name { set; get; }
		
		public bool isDeleted { set; get; } = false;
		public virtual List<City> Cities { set; get; }
	}
}
