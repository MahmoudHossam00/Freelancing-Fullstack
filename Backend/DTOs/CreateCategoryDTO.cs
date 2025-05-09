namespace Freelancing.DTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
