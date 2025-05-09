namespace Freelancing.IRepositoryService
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesBycategoryIdAsync(int categoryid);
        Task<Subcategory?> GetSubcategoryByIdAsync(int id);
        Task<bool> CreateSubcategoryAsync(Subcategory subCategory);
        Task<bool> UpdateSubcategoryAsync(Subcategory subCategory);
        Task<bool> DeleteSubcategoryAsync(int id);
        Task<bool> IsSubcategoryExistsAsync(string name);
        Task<bool> IsSubcategoryExistsByIdAsync(int id);
    }
}
