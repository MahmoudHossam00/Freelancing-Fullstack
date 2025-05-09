using System.ComponentModel.DataAnnotations;

namespace Freelancing.DTOs
{
    public class SkillDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(100, ErrorMessage = "Skill name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
    }

 
}
