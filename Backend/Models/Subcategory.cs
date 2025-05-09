using System.ComponentModel.DataAnnotations.Schema;
namespace Freelancing.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}