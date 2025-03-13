using CoffeeAPI.Datas;
using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByIdAsync(int id, string includeProperties = null);
        // Lấy danh sách sản phẩm theo danh mục
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, string includeProperties = null);
        // Lấy sản phẩm theo trạng thái (in_stock/out_of_stock)
        Task<IEnumerable<Product>> GetByStatusAsync(string status, string includeProperties = null);
        // Tìm kiếm sản phẩm theo tên (bao gồm tìm kiếm không phân biệt hoa thường)
        Task<IEnumerable<Product>> SearchByNameAsync(string name, string includeProperties = null);
        // Lấy sản phẩm bán chạy nhất (giả sử dựa trên số lượng đã bán)
        Task<Product> GetBestSellerAsync(string includeProperties = null);
        // Cập nhật trạng thái sản phẩm (in_stock/out_of_stock)
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
            return await Get(p => p.ProductId == id, includeProperties: includeProperties).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId, string includeProperties = null)
        {
            return await Get(p => p.CategoryId == categoryId, includeProperties: includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync(string status, string includeProperties = null)
        {
            return await Get(p => p.Status == status, includeProperties: includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name, string includeProperties = null)
        {
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

            return await GetByIdAsync(bestSeller.ProductId, includeProperties);
        }

        public async Task UpdateProductStatusAsync(int productId, string status)
        {
            var product = await GetByIdAsync(productId, includeProperties: "Category");
            if (product != null)
            {
                product.Status = status;
                Update(product);
                await SaveChangesAsync();
            }
        }
    }
}
