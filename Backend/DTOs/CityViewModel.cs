
using Freelancing.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs
{
	public class CityViewModel
	{
		public int? Id { set; get; }
		public string Name { set; get; }


		public int CountryId { get; set; }
	}
}
