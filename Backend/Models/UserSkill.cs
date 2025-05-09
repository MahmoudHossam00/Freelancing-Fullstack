using Freelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class UserSkill
{
    public int id { get; set; }

	[ForeignKey("Freelancer")]
    public string FreelancerId { get; set; }
    public bool IsDelete { get; set; }
    public Freelancer Freelancer { get; set; }
    [ForeignKey("Skill")]
    public int SkillId { get; set; }
    public Skill Skill { get; set; }
}