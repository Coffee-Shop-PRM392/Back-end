using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetByIdAsync(int id, string includeProperties = null);
        // Lấy giỏ hàng của một user
        Task<IEnumerable<Cart>> GetByUserAsync(int userId);

        // Xóa toàn bộ giỏ hàng của một user (sau khi đặt hàng)
        Task ClearCartAsync(int userId);

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        Task UpdateQuantityAsync(int cartId, int quantity);
    }
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(CoffeeSalesContext context) : base(context)
        {
        }
        public async Task<Cart> GetByIdAsync(int id, string includeProperties = null)
        {
            return await Get(oi => oi.CartId == id, includeProperties: includeProperties).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Cart>> GetByUserAsync(int userId)
        {
            return await Get(c => c.UserId == userId, includeProperties: "User,Product").ToListAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await GetByUserAsync(userId);
            foreach (var item in cartItems)
            {
                Delete(item);
            }
            await SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int cartId, int quantity)
        {
            var cartItem = await GetByIdAsync(cartId, includeProperties: "Product"); 
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                Update(cartItem);
                await SaveChangesAsync();
            }
        }
    }
}
