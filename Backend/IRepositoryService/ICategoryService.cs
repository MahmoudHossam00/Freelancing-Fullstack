namespace Freelancing.IRepositoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetAllCategoriesBySuncategoryNameAsync(string subcategoryname);
        Task<IEnumerable<Category>> GetAllCategoriesBySuncategoryIdAsync(int subcategoryid);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> IsCategoryExistsAsync(string name);
        Task<bool> IsCategoryExistsByIdAsync(int id);
    }
}
