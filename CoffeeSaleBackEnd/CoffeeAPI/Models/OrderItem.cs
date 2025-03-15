using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceAtPurchase { get; set; }

        [Required]
        [StringLength(10)]
        public string SelectedSize { get; set; }  // "S", "M", "L"

        [StringLength(20)]
        public string IceLevel { get; set; } = "normal";  // "low", "none", "normal"

        [StringLength(20)]
        public string SugarLevel { get; set; } = "normal";  // "low", "none", "normal"
    }
}
