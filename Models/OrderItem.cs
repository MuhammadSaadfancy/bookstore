using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;

namespace BookStore.Models
{
	public class OrderItem
	{
		[Key]
		public int OrderItemId { get; set; }

		[Required]
		public int OrderId { get; set; }

		[ForeignKey("OrderId")]
		public Order? Order { get; set; }

		[Required]
		public int ProductId { get; set; }

		[ForeignKey("ProductId")]
		public Product? Product { get; set; }

		// 7-digit product code
		public string ProductCode { get; set; } = "";

		[Required]
		public string ProductName { get; set; } = "";

		[Required]
		[Range(0.01, 1000000)]
		public decimal Price { get; set; }

		[Required]
		[Range(1, 1000)]
		public int Quantity { get; set; }

		[NotMapped]
		public decimal Subtotal => Price * Quantity;
	}
}