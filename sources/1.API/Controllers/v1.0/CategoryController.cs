using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;

namespace API.Controllers.v1_0
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Category APIs")]
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost(Name = "POST - Entrypoint for create category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            var createdCategory = _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategory.Id }, createdCategory);
        }

        [HttpGet(Name = "GET - Entrypoint for get all Categorie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{categoryId}", Name = "GET - Entrypoint for get Category by Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut("{categoryId}", Name = "PUT - Entrypoint for update Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category updatedCategory)
        {
            try
            {
                var category = _categoryService.UpdateCategory(updatedCategory);

                return Ok(category);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{categoryId}", Name = "DELETE - Entrypoint for remove Category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                var deletedCategory = _categoryService.DeleteCategory(categoryId);

                return Ok(deletedCategory);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
