using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoffeeAPI.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }  // Lưu ý: Mã hóa trước khi lưu

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "customer";  // customer hoặc admin

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ 1-nhiều với Cart và Orders
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
