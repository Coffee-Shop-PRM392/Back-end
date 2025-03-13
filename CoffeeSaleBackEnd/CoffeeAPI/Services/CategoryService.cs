using CoffeeAPI.Models;
using CoffeeAPI.Repository;

namespace CoffeeAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryRepository.GetByIdAsync(categoryId);
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _categoryRepository.GetByNameAsync(name);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            // Logic nghiệp vụ: Kiểm tra tên danh mục đã tồn tại chưa
            if (await _categoryRepository.NameExistsAsync(category.Name))
            {
                throw new InvalidOperationException("Category name already exists.");
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
