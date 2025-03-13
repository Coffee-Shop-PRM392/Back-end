using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "pending";  // pending, confirmed, completed, canceled

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; } = "unpaid";  // unpaid, paid, failed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ 1-nhiều với OrderItems
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
