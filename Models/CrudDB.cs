using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Models
{
	public class CrudDB : DbContext
	{
		public CrudDB(DbContextOptions<CrudDB> options) : base(options)
		{
		}

		// Yeh sab DbSet same rakho
		public DbSet<Admin> tbl_Admin { get; set; }
		public DbSet<Customer> tbl_Customer { get; set; }
		public DbSet<Category> tbl_Category { get; set; }
		public DbSet<Cart> tbl_Cart { get; set; }
		public DbSet<Feedback> tbl_Feedback { get; set; }
		public DbSet<Faqs> tbl_Faqs { get; set; }
		public DbSet<Order> tbl_order { get; set; }
		public DbSet<OrderItem> tbl_orderitem { get; set; }
		public DbSet<Product> tbl_Product { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// OrderItem aur Product ka relation (yeh sab se important hai)
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Product)
				.WithMany()
				.HasForeignKey(oi => oi.ProductId)
				.OnDelete(DeleteBehavior.Restrict);  // Delete time pe restrict (safety)

			// Order aur Customer ka relation (CustomerId ke liye)
			modelBuilder.Entity<Order>()
				.HasOne(o => o.Customer)
				.WithMany()
				.HasForeignKey(o => o.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			// Cart aur Product ka relation (prod_id ke liye)
			modelBuilder.Entity<Cart>()
				.HasOne(c => c.Product)
				.WithMany()
				.HasForeignKey(c => c.prod_id)
				.OnDelete(DeleteBehavior.Restrict);

			// Agar aur relations configure karne hain to yahan add kar sakte ho
		}
	}
}