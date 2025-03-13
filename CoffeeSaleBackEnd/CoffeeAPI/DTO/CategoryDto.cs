namespace CoffeeAPI.DTO
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CategoryRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductCount { get; set; } // Số lượng sản phẩm thuộc danh mục
    }
}
