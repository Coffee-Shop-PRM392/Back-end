using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByIdAsync(int id, string includeProperties = null);
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, string includeProperties = null);
        Task<IEnumerable<Product>> GetByStatusAsync(string status, string includeProperties = null);
        Task<IEnumerable<Product>> SearchByNameAsync(string name, string includeProperties = null);
        Task<Product> GetBestSellerAsync(string includeProperties = null);
        Task UpdateProductStatusAsync(int productId, string status);
    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;

        public ProductRepository(CoffeeSalesContext context, IGenericRepository<OrderItem> orderItemRepository) : base(context)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<Product> GetByIdAsync(int id, string includeProperties = null)
        {
            // Mặc định bao gồm ProductSizes
            string defaultIncludes = "ProductSizes";
            includeProperties = string.IsNullOrEmpty(includeProperties)
                ? defaultIncludes
                : $"{defaultIncludes},{includeProperties}";
            return await Get(p => p.ProductId == id, includeProperties: includeProperties).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, string includeProperties = null)
        {
            string defaultIncludes = "ProductSizes";
            includeProperties = string.IsNullOrEmpty(includeProperties)
                ? defaultIncludes
                : $"{defaultIncludes},{includeProperties}";
            return await Get(p => p.CategoryId == categoryId, includeProperties: includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync(string status, string includeProperties = null)
        {
            string defaultIncludes = "ProductSizes";
            includeProperties = string.IsNullOrEmpty(includeProperties)
                ? defaultIncludes
                : $"{defaultIncludes},{includeProperties}";
            return await Get(p => p.Status == status, includeProperties: includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name, string includeProperties = null)
        {
            string defaultIncludes = "ProductSizes";
            includeProperties = string.IsNullOrEmpty(includeProperties)
                ? defaultIncludes
                : $"{defaultIncludes},{includeProperties}";
            return await Get(p => p.Name.ToLower().Contains(name.ToLower()), includeProperties: includeProperties).ToListAsync();
        }

        public async Task<Product> GetBestSellerAsync(string includeProperties = null)
        {
            var orderItems = await _orderItemRepository.GetAllAsync();

            var bestSeller = orderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(oi => oi.Quantity) })
                .OrderByDescending(x => x.TotalSold)
                .FirstOrDefault();

            if (bestSeller == null)
            {
                return null;
            }

            string defaultIncludes = "ProductSizes";
            includeProperties = string.IsNullOrEmpty(includeProperties)
                ? defaultIncludes
                : $"{defaultIncludes},{includeProperties}";
            return await GetByIdAsync(bestSeller.ProductId, includeProperties);
        }

        public async Task UpdateProductStatusAsync(int productId, string status)
        {
            var product = await GetByIdAsync(productId, "Category");
            if (product != null)
            {
                product.Status = status;
                Update(product);
                await SaveChangesAsync();
            }
        }
    }
}