using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
    public class ProjectSkill
    {
        public int id { get; set; }



        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }



        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }


        public bool IsDelete { get; set; }
    }
}
