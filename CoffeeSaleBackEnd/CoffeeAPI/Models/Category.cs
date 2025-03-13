using System.ComponentModel.DataAnnotations;

namespace CoffeeAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Quan hệ 1-nhiều với Products
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
