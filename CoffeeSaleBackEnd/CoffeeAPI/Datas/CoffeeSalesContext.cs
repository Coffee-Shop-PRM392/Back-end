using CoffeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAPI.Datas
{
    public class CoffeeSalesContext : DbContext
    {
        public CoffeeSalesContext(DbContextOptions<CoffeeSalesContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Đảm bảo email của User là duy nhất
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Đảm bảo một sản phẩm chỉ xuất hiện 1 lần trong giỏ hàng của 1 người
            modelBuilder.Entity<Cart>()
                .HasIndex(c => new { c.UserId, c.ProductId })
                .IsUnique();
        }
    }
}
