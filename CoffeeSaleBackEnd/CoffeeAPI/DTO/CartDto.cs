namespace CoffeeAPI.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
        public DateTime AddedAt { get; set; }
    }

    // DTO cho request thêm/sửa Cart
    public class CartRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
    }

    // DTO cho response trả về Cart
    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string SelectedSize { get; set; }  // "S", "M", "L"
        public string IceLevel { get; set; }  // "low", "none", "normal"
        public string SugarLevel { get; set; }  // "low", "none", "normal"
        public DateTime AddedAt { get; set; }
    }
}
