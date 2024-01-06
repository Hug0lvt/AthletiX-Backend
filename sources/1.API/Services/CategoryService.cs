using Model;
using Repositories;
using API.Exceptions;

namespace API.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category CreateCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category;
        }

        public List<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public Category UpdateCategory(Category updatedCategory)
        {
            var existingCategory = _dbContext.Categories.Find(updatedCategory.Id);

            if (existingCategory != null)
            {
                existingCategory.Title = updatedCategory.Title;
                _dbContext.SaveChanges();
                return existingCategory;
            }

            throw new NotFoundException("[LOG | CategoryService] - (UpdateCategory) : Category not found");
        }

        public Category DeleteCategory(int categoryId)
        {
            var categoryToDelete = _dbContext.Categories.Find(categoryId);

            if (categoryToDelete != null)
            {
                _dbContext.Categories.Remove(categoryToDelete);
                _dbContext.SaveChanges();
                return categoryToDelete;
            }

            throw new NotFoundException("[LOG | CategoryService] - (DeleteCategory) : Category not found");
        }
    }
}
