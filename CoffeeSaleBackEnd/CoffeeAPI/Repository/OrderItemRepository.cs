﻿using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<OrderItem> GetByIdAsync(int id, string includeProperties = null);
        Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetByProductAsync(int productId);
    }

    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(CoffeeSalesContext context) : base(context)
        {
        }

        public async Task<OrderItem> GetByIdAsync(int id, string includeProperties = null)
        {
            return await Get(oi => oi.OrderItemId == id, includeProperties: includeProperties).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId)
        {
            return await Get(oi => oi.OrderId == orderId, includeProperties: "Order,Product,Product.ProductSizes").ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByProductAsync(int productId)
        {
            return await Get(oi => oi.ProductId == productId, includeProperties: "Product,Product.ProductSizes").ToListAsync();
        }
    }
}