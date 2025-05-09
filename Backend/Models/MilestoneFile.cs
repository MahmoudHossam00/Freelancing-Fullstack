using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
	public class MilestoneFile
	{
		public int id { set; get; }
		public string fileName { set; get; }


		[ForeignKey("Milestone")]
		public int MilestoneId { set; get; }
		public Milestone Milestone { set; get; }
	}
}
