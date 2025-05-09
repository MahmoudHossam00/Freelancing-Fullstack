using Freelancing.DTOs;
//using Freelancing.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Freelancing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }





        [HttpPost]
        public async Task<ActionResult> CreateCategory(CreateCategoryDTO category)
        {
           
            var newCategory = new Category
            {
                Name = category.Name,
                IsDeleted = false,
            };

            var createdCategory = await _categoryService.CreateCategoryAsync(newCategory);
            return Ok ( new 
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
               
            });
        }




        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            if (categories == null)
            {
                return BadRequest(new { Message = "No categories found." });
            }
            var categoriesDto = categories.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleted = c.IsDeleted,


            }); return Ok(categoriesDto);



        }



        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return BadRequest(new { Message = $"Category with ID {id} not found." });
            }
            var categoryDto = new
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
            };
            return Ok(categoryDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory( UpdateCategoryDTO category)
        {
            if (category == null)
            {
                return BadRequest(new { Message = "Category cannot be null." });
            }
            var existingCategory = await _categoryService.GetCategoryByIdAsync(category.Id);
            if (existingCategory == null)
            {
                return BadRequest(new { Message = $"Category with ID {category.Id} not found." });
            }
            existingCategory.Name = category.Name;
            existingCategory.IsDeleted = category.IsDeleted;
            var result = await _categoryService.UpdateCategoryAsync(existingCategory);
           
            return Ok(new
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                IsDeleted = existingCategory.IsDeleted,
            });




        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return BadRequest(new { Message = $"Category with ID {id} not found." });
            }
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to delete category." });
            }
            return Ok(new {
                Message=

            $"Category with ID {id} deleted successfully."});
            
        }





    }
}
