using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Lấy danh mục theo tên
        Task<Category> GetByNameAsync(string name);

        // Kiểm tra xem danh mục đã tồn tại chưa
        Task<bool> NameExistsAsync(string name);
    }
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CoffeeSalesContext context) : base(context)
        {
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await Get(c => c.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await ExistsAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
