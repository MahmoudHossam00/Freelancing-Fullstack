using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.DTOs
{
    public class GetAllSubCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
     
        public int CategoryId { get; set; }
    }
}
