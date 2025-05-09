
namespace Freelancing.RepositoryService
{
    public class CategoryService(ApplicationDbContext _context) : ICategoryService
    {
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _context.categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category= await GetCategoryByIdAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                _context.categories.Remove(category);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _context.categories
                .Include(c => c.Subcategories)                
                .ToListAsync();
            if(categories.Count == 0)
            {
                return null;
            }
            return categories;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesBySuncategoryNameAsync(string subcategoryname)
        {
            var categories = await _context.categories
                .Include(c => c.Subcategories)
                .Where(c => c.Subcategories.Any(sc => sc.Name == subcategoryname))
                .ToListAsync();
            if (categories.Count == 0)
            {
                return null;
            }
            return categories;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesBySuncategoryIdAsync(int subcategoryid)
        {
            var categories = await _context.categories
                .Include(c => c.Subcategories)
                .Where(c => c.Subcategories.Any(sc => sc.Id == subcategoryid))
                .ToListAsync();
            if (categories.Count == 0)
            {
                return null;
            }
            return categories;
        }
     

        public async Task<bool> IsCategoryExistsAsync(string name)
        {            
            return await _context.categories
                .AnyAsync(c => c.Name == name);
        }

        public async Task<bool> IsCategoryExistsByIdAsync(int id)
        {
            return await _context.categories
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            var selectedcategory = await GetCategoryByIdAsync(category.Id);
            if (selectedcategory != null)
            {
                 _context.categories.Update(category);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            var selectedcategory = await _context.categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (selectedcategory != null)
            {
                return selectedcategory;
            }
            return null;
        }
    }
}
