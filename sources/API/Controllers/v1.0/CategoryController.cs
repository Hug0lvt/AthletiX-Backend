using Microsoft.AspNetCore.Mvc;
using API.Services;
using Model;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.v1_0
{
    /// <summary>
    /// Controller for managing operations related to categories.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Category APIs")]
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category to create.</param>
        /// <returns>The newly created category.</returns>
        [HttpPost(Name = "POST - Entrypoint for create category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            var createdCategory = _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategory.Id }, createdCategory);
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        [HttpGet(Name = "GET - Entrypoint for get all Categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        /// <summary>
        /// Gets all categories with pages.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        [HttpGet("pages", Name = "GET - Entrypoint for get all Categories with pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllCategoriesWithPages(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 0)
        {
            var categories = _categoryService.GetAllCategoriesWithPages(pageSize, pageNumber);
            return Ok(categories);
        }

        /// <summary>
        /// Gets a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The category with the specified identifier.</returns>
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

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="updatedCategory">The updated category information.</param>
        /// <returns>The updated category.</returns>
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

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The deleted category.</returns>
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
