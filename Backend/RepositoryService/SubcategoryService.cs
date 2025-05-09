
namespace Freelancing.RepositoryService
{
    public class SubcategoryService(ApplicationDbContext _context) : ISubcategoryService
    {
        public async Task<bool> CreateSubcategoryAsync(Subcategory subCategory)
        {
            await _context.Subcategories.AddAsync(subCategory);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSubcategoryAsync(int id)
        {
            var selectedsubcategory = await GetSubcategoryByIdAsync(id);
            if (selectedsubcategory != null)
            {
                selectedsubcategory.IsDeleted = true;
                _context.Subcategories.Remove(selectedsubcategory);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
        {
            var subcategories = await _context.Subcategories
                .Include(c => c.Category)                
                .ToListAsync();
            return subcategories;
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesBycategoryIdAsync(int categoryid)
        {
            var subcategories = await _context.Subcategories
                .Include(c => c.Category)
                .Where(c => c.CategoryId == categoryid)
                .ToListAsync();
            return subcategories;
        }

        public async Task<Subcategory?> GetSubcategoryByIdAsync(int id)
        {
            var subcategory = await _context.Subcategories
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            return subcategory;
        }

        public async Task<bool> IsSubcategoryExistsAsync(string name)
        {
            return await _context.Subcategories
                .AnyAsync(c => c.Name == name);
        }

        public async Task<bool> IsSubcategoryExistsByIdAsync(int id)
        {
            return await _context.Subcategories
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateSubcategoryAsync(Subcategory subCategory)
        {
            var selectedsubcategory = await GetSubcategoryByIdAsync(subCategory.Id);
            if (selectedsubcategory != null)
            {
                _context.Subcategories.Update(subCategory);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
