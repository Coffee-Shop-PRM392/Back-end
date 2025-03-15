namespace CoffeeAPI.DTO
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }  // Giá dựa trên size đã chọn
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
    }

    // DTO cho request tạo/sửa OrderItem
    public class OrderItemRequestDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
    }

    // DTO cho response trả về OrderItem
    public class OrderItemResponseDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
    }
}
