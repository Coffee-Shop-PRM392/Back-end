namespace CoffeeAPI.DTO
{
    // DTO chính của Product
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; } = new List<ProductSizeDto>(); // Danh sách size và giá
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // DTO cho request tạo/sửa Product
    public class ProductRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; } = new List<ProductSizeDto>(); // Danh sách size và giá
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
    }

    // DTO cho response trả về Product
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; } = new List<ProductSizeDto>(); // Danh sách size và giá
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // DTO mới cho thông tin size và giá
    public class ProductSizeDto
    {
        public string Size { get; set; }  // "S", "M", "L"
        public decimal Price { get; set; }  // Giá cho từng size
    }
}
