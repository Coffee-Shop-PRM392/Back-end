using CoffeeAPI.Models;
using CoffeeAPI.Repository;

namespace CoffeeAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task DeleteOrderAsync(int orderId);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _orderRepository.GetByUserAsync(userId, includeProperties: "User");
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _orderRepository.GetByStatusAsync(status, includeProperties: "User");
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId, includeProperties: "User");
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Logic nghiệp vụ: Kiểm tra tổng tiền
            if (order.TotalAmount <= 0)
            {
                throw new ArgumentException("Total amount must be greater than 0.");
            }

            // Thêm Order
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // Thêm các OrderItems (nếu có)
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    item.OrderId = order.OrderId;
                    await _orderItemRepository.AddAsync(item);
                }
                await _orderItemRepository.SaveChangesAsync();
            }

            return await _orderRepository.GetByIdAsync(order.OrderId, includeProperties: "User");
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, includeProperties: "User"); 
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
