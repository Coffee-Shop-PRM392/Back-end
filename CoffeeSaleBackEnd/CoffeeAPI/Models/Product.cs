    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    namespace CoffeeAPI.Models
    {
        public class Product
        {
            [Key]
            public int ProductId { get; set; }

            [Required]
            public int CategoryId { get; set; }

            [ForeignKey("CategoryId")]
            public Category Category { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            public string? Description { get; set; }

            [StringLength(800)]
            public string? ImageUrl { get; set; }

            [StringLength(20)]
            public string Status { get; set; } = "in_stock";  // in_stock hoặc out_of_stock

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ 1-nhiều với Cart và OrderItems và ProductSizes
            public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
            public List<Cart> Carts { get; set; } = new List<Cart>();
            public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        }
    }