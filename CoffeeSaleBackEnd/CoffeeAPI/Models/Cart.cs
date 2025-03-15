using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeAPI.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [StringLength(10)]
        public string SelectedSize { get; set; }  // "S", "M", "L"

        [StringLength(20)]
        public string IceLevel { get; set; } = "normal";  // "low", "none", "normal"

        [StringLength(20)]
        public string SugarLevel { get; set; } = "normal";  // "low", "none", "normal"

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
