using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        // Lấy user theo email
        Task<Users> GetByEmailAsync(string email);

        // Kiểm tra xem email đã tồn tại chưa
        Task<bool> EmailExistsAsync(string email);

        // Lấy danh sách user theo vai trò (customer/admin)
        Task<IEnumerable<Users>> GetByRoleAsync(string role);
    }
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        public UserRepository(CoffeeSalesContext context) : base(context)
        {
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            return await Get(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await ExistsAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Users>> GetByRoleAsync(string role)
        {
            return await Get(u => u.Role == role).ToListAsync();
        }
    }
}
