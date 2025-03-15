using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetByIdAsync(int id, string includeProperties = null);
        Task<IEnumerable<Cart>> GetByUserAsync(int userId);
        Task ClearCartAsync(int userId);
        Task UpdateQuantityAsync(int cartId, int quantity);
        // Thêm phương thức mới để lấy Cart theo Product và tùy chọn
        Task<Cart> GetByProductAndOptionsAsync(int userId, int productId, string size, string iceLevel, string sugarLevel);
    }

    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(CoffeeSalesContext context) : base(context)
        {
        }

        public async Task<Cart> GetByIdAsync(int id, string includeProperties = null)
        {
            return await Get(c => c.CartId == id, includeProperties: includeProperties).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cart>> GetByUserAsync(int userId)
        {
            return await Get(c => c.UserId == userId, includeProperties: "User,Product,Product.ProductSizes").ToListAsync();
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
            var cartItem = await GetByIdAsync(cartId, includeProperties: "Product,Product.ProductSizes");
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                Update(cartItem);
                await SaveChangesAsync();
            }
        }

        public async Task<Cart> GetByProductAndOptionsAsync(int userId, int productId, string size, string iceLevel, string sugarLevel)
        {
            return await Get(c => c.UserId == userId
                && c.ProductId == productId
                && c.SelectedSize == size
                && c.IceLevel == iceLevel
                && c.SugarLevel == sugarLevel,
                includeProperties: "Product,Product.ProductSizes").FirstOrDefaultAsync();
        }
    }
}