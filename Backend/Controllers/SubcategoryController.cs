using Freelancing.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {

        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;
        public SubcategoryController(ISubcategoryService subcategoryService, ICategoryService categoryService)
        {
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateSubcategory(CreateSubcategoryDTO subcategory)
        {
            //validate categoryid is found

            var category = await _categoryService.GetCategoryByIdAsync(subcategory.CategoryId);
            if (category == null)
            {
                return BadRequest(new { Message = "Category not found." });
            }

            var newSubcategory = new Subcategory
            {
                Name = subcategory.Name,
                CategoryId = subcategory.CategoryId,
                IsDeleted = false,
            };
            var createdSubcategory = await _subcategoryService.CreateSubcategoryAsync(newSubcategory);
            return Ok(new
            {
                Id = newSubcategory.Id,
                Name = newSubcategory.Name,
                CategoryId = newSubcategory.CategoryId,
                IsDeleted = false,
            });
        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllSubCategoryDTO>>> GetAllSubcategories()

        {
            var subcategories = await _subcategoryService.GetAllSubcategoriesAsync();
            if (subcategories == null)
            {
                return BadRequest(new { Message = "No subcategories found." });
            }
          

            var subcategoriesdto= subcategories.Select(s => new GetAllSubCategoryDTO
           
            {
                Id = s.Id,
                Name = s.Name,
                CategoryId = s.CategoryId,
                IsDeleted = s.IsDeleted,
            });


            return Ok(subcategoriesdto);
        }



        [HttpGet("{id}")]

        public async Task<ActionResult> GetSubcategoryById(int id)
        {
            var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (subcategory == null)
            {
                return BadRequest(new { Message = "Subcategory not found." });
            }
            var subcategorydto = new GetAllSubCategoryDTO
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                CategoryId = subcategory.CategoryId,
                IsDeleted = subcategory.IsDeleted,
            };
            return Ok(subcategorydto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubcategoryDTO (int id, CreateSubcategoryDTO subcategory) 
        {
        
            var existingsubcategory= await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (existingsubcategory == null)
            {
                return BadRequest(new { Message = "Subcategory not found." });
            }
            var category = await _categoryService.GetCategoryByIdAsync(subcategory.CategoryId);
            if (category == null)
            {
                return BadRequest(new { Message = "Category not found." });
            }



            existingsubcategory.Name = subcategory.Name;
            existingsubcategory.CategoryId = subcategory.CategoryId;
          //  existingsubcategory.IsDeleted = subcategory.IsDeleted;
            var updatedSubcategory = await _subcategoryService.UpdateSubcategoryAsync(existingsubcategory);
            if (!updatedSubcategory)
            {
                return BadRequest(new { Message = "Failed to update subcategory." });
            }
            return Ok  (new
            {
                Id = id,
                Name = existingsubcategory.Name,
                CategoryId = existingsubcategory.CategoryId,
                IsDeleted =false,
            });



        }






        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubcategory(int id)
        {
            var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);
            if (subcategory == null)
            {
                return BadRequest(new { Message = "Subcategory not found." });
            }
            var deletedSubcategory = await _subcategoryService.DeleteSubcategoryAsync(id);
            if (!deletedSubcategory)
            {
                return BadRequest(new { Message = "Failed to delete subcategory." });
            }
            return Ok(new { Message = "Subcategory deleted successfully." });
        }








    }
}
