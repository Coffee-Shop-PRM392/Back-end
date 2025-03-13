namespace CoffeeAPI.DTO
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string Customizations { get; set; }
    }
    public class OrderItemRequestDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string Customizations { get; set; }
    }

    public class OrderItemResponseDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Tên sản phẩm thay vì object Product
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string Customizations { get; set; }
    }
}
