using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetByIdAsync(int orderId, string includeProperties = null);
        // Lấy danh sách đơn hàng của một user
        Task<IEnumerable<Order>> GetByUserAsync(int userId, string includeProperties = null);
        // Lấy đơn hàng theo trạng thái (pending, confirmed, completed, canceled)
        Task<IEnumerable<Order>> GetByStatusAsync(string status, string includeProperties = null);
        // Cập nhật trạng thái đơn hàng
        Task UpdateOrderStatusAsync(int orderId, string status);
    }
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CoffeeSalesContext context) : base(context)
        {
        }
        public async Task<Order> GetByIdAsync(int orderId, string includeProperties = null)
        {
            return await Get(o => o.OrderId == orderId, includeProperties: includeProperties).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Order>> GetByUserAsync(int userId, string includeProperties = null)
        {
            return await Get(o => o.UserId == userId, includeProperties: includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(string status, string includeProperties = null)
        {
            return await Get(o => o.Status == status, includeProperties: includeProperties).ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await GetByIdAsync(orderId, includeProperties: "User"); 
            if (order != null)
            {
                order.Status = status;
                Update(order);
                await SaveChangesAsync();
            }
        }
    }
}
