using CoffeeAPI.Models;
using CoffeeAPI.Repository;

namespace CoffeeAPI.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartByUserAsync(int userId);
        Task<Cart> AddToCartAsync(Cart cart);
        Task<Cart> UpdateCartQuantityAsync(int cartId, int quantity); // Thay đổi từ Task thành Task<Cart>
        Task ClearCartAsync(int userId);
        Task RemoveFromCartAsync(int cartId);
    }
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<Cart>> GetCartByUserAsync(int userId)
        {
            return await _cartRepository.GetByUserAsync(userId);
        }

        public async Task<Cart> AddToCartAsync(Cart cart)
        {
            // Logic nghiệp vụ: Kiểm tra số lượng
            if (cart.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = await _cartRepository.GetAsync(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cart.Quantity;
                _cartRepository.Update(existingCartItem);
            }
            else
            {
                await _cartRepository.AddAsync(cart);
            }

            await _cartRepository.SaveChangesAsync();
            int cartId = existingCartItem?.CartId ?? cart.CartId;
            return await _cartRepository.GetByIdAsync(cartId, includeProperties: "Product");
        }

        public async Task<Cart> UpdateCartQuantityAsync(int cartId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            await _cartRepository.UpdateQuantityAsync(cartId, quantity);
            return await _cartRepository.GetByIdAsync(cartId, includeProperties: "Product");
        }
        public async Task ClearCartAsync(int userId)
        {
            await _cartRepository.ClearCartAsync(userId);
        }

        public async Task RemoveFromCartAsync(int cartId)
        {
            var cartItem = await _cartRepository.GetByIdAsync(cartId, includeProperties: "Product");
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            _cartRepository.Delete(cartItem);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
