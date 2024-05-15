using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using API.Exceptions;
using API.Repositories;
using Shared.Mappers;

namespace API.Services
{
    /// <summary>
    /// Service for managing categories.
    /// </summary>
    public class CategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IdentityAppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public CategoryService(IdentityAppDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category to be created.</param>
        /// <returns>The created category.</returns>
        public Category CreateCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public List<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }

        /// <summary>
        /// Gets all categories (with pages).
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public PaginationResult<Category> GetAllCategoriesWithPages(
            int pageSize = 10,
            int pageNumber = 0)
        {
            var totalItems = _dbContext.Categories.Count();
            var items = _dbContext.Categories
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationResult<Category>
            {
                Items = items,
                NextPage = (pageNumber + 1) * pageSize < totalItems ? pageNumber + 1 : -1,
                TotalItems = totalItems
            };
        }

        /// <summary>
        /// Gets a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the category.</param>
        /// <returns>The category with the specified identifier.</returns>
        public Category GetCategoryById(int categoryId)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="updatedCategory">The updated category information.</param>
        /// <returns>The updated category.</returns>
        public Category UpdateCategory(Category updatedCategory)
        {
            var existingCategory = _dbContext.Categories.Find(updatedCategory.Id);

            if (existingCategory != null)
            {
                existingCategory.Title = updatedCategory.Title;
                _dbContext.SaveChanges();
                return existingCategory;
            }

            _logger.LogTrace("[LOG | CategoryService] - (UpdateCategory): Category not found");
            throw new NotFoundException("[LOG | CategoryService] - (UpdateCategory): Category not found");
        }

        /// <summary>
        /// Deletes a category by its identifier.
        /// </summary>
        /// <param name="categoryId">The identifier of the category to be deleted.</param>
        /// <returns>The deleted category.</returns>
        public Category DeleteCategory(int categoryId)
        {
            var categoryToDelete = _dbContext.Categories.Find(categoryId);

            if (categoryToDelete != null)
            {
                _dbContext.Categories.Remove(categoryToDelete);
                _dbContext.SaveChanges();
                return categoryToDelete;
            }

            _logger.LogTrace("[LOG | CategoryService] - (DeleteCategory): Category not found");
            throw new NotFoundException("[LOG | CategoryService] - (DeleteCategory): Category not found");
        }
    }
}
