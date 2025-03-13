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

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "in_stock";  // in_stock hoặc out_of_stock

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ 1-nhiều với Cart và OrderItems
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
