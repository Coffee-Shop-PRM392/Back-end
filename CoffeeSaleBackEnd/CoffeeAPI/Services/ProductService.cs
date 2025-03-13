using CoffeeAPI.Models;
using CoffeeAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int pageIndex, int pageSize, string status = null);
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
        Task<Product> GetBestSellerAsync();
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task UpdateProductStatusAsync(int productId, string status);
        Task DeleteProductAsync(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int pageIndex, int pageSize, string status = null)
        {
            return await _productRepository.Get(
                filter: status != null ? p => p.Status == status : null,
                orderBy: q => q.OrderBy(p => p.Name),
                includeProperties: "Category",
                pageIndex: pageIndex,
                pageSize: pageSize
            ).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId, includeProperties: "Category");
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryAsync(categoryId, includeProperties: "Category");
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
        {
            return await _productRepository.SearchByNameAsync(name, includeProperties: "Category");
        }

        public async Task<Product> GetBestSellerAsync()
        {
            return await _productRepository.GetBestSellerAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Logic nghiệp vụ: Kiểm tra giá sản phẩm
            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than 0.");
            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return await _productRepository.GetByIdAsync(product.ProductId, includeProperties: "Category");
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;

            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateProductStatusAsync(int productId, string status)
        {
            await _productRepository.UpdateProductStatusAsync(productId, status);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
