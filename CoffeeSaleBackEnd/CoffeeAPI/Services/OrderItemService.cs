using CoffeeAPI.Models;
using CoffeeAPI.Repository;

namespace CoffeeAPI.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByProductAsync(int productId);
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int orderItemId);
    }

    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId)
        {
            return await _orderItemRepository.GetByOrderAsync(orderId);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByProductAsync(int productId)
        {
            return await _orderItemRepository.GetByProductAsync(productId);
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            // Logic nghiệp vụ: Kiểm tra số lượng và các tùy chọn
            if (orderItem.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }
            if (orderItem.PriceAtPurchase <= 0)
            {
                throw new ArgumentException("Price at purchase must be greater than 0.");
            }
            if (string.IsNullOrEmpty(orderItem.SelectedSize) || string.IsNullOrEmpty(orderItem.IceLevel) || string.IsNullOrEmpty(orderItem.SugarLevel))
            {
                throw new ArgumentException("Size, ice level, and sugar level must be specified.");
            }

            await _orderItemRepository.AddAsync(orderItem);
            await _orderItemRepository.SaveChangesAsync();
            return await _orderItemRepository.GetByIdAsync(orderItem.OrderItemId, includeProperties: "Product,Product.ProductSizes");
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId, includeProperties: "Product,Product.ProductSizes");
            if (orderItem == null)
            {
                throw new KeyNotFoundException("OrderItem not found.");
            }

            _orderItemRepository.Delete(orderItem);
            await _orderItemRepository.SaveChangesAsync();
        }
    }
}