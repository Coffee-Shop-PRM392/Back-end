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
                includeProperties: "Category,ProductSizes",
                pageIndex: pageIndex,
                pageSize: pageSize
            ).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId, includeProperties: "Category,ProductSizes");
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryAsync(categoryId, includeProperties: "Category,ProductSizes");
        }

        public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string name)
        {
            return await _productRepository.SearchByNameAsync(name, includeProperties: "Category,ProductSizes");
        }

        public async Task<Product> GetBestSellerAsync()
        {
            return await _productRepository.GetBestSellerAsync(includeProperties: "Category,ProductSizes");
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Logic nghiệp vụ: Kiểm tra giá trong ProductSizes
            if (product.ProductSizes == null || !product.ProductSizes.Any() || product.ProductSizes.Any(ps => ps.Price <= 0))
            {
                throw new ArgumentException("Product must have at least one size with a price greater than 0.");
            }

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
            return await _productRepository.GetByIdAsync(product.ProductId, includeProperties: "Category,ProductSizes");
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.ProductId, includeProperties: "ProductSizes");
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            // Cập nhật thông tin cơ bản
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;

            // Cập nhật ProductSizes
            existingProduct.ProductSizes.Clear();
            foreach (var size in product.ProductSizes)
            {
                size.ProductId = existingProduct.ProductId; // Gắn ProductId cho ProductSize
                existingProduct.ProductSizes.Add(size);
            }

            if (!existingProduct.ProductSizes.Any() || existingProduct.ProductSizes.Any(ps => ps.Price <= 0))
            {
                throw new ArgumentException("Product must have at least one size with a price greater than 0.");
            }

            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateProductStatusAsync(int productId, string status)
        {
            await _productRepository.UpdateProductStatusAsync(productId, status);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId, includeProperties: "ProductSizes");
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}