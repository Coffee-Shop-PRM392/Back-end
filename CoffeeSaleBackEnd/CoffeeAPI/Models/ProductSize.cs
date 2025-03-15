using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeAPI.Models
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [StringLength(10)]
        public string Size { get; set; }  // "S", "M", "L"

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }  // Giá cho từng size
    }
}
