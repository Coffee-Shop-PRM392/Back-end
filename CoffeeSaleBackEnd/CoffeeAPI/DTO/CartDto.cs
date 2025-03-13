namespace CoffeeAPI.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Customizations { get; set; }
        public DateTime AddedAt { get; set; }
    }
    public class CartRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Customizations { get; set; }
    }

    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; } // Tên sản phẩm thay vì object Product
        public int Quantity { get; set; }
        public string Customizations { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
