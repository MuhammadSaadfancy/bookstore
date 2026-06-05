using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
	public class Order
	{
		[Key]
		public int OrderId { get; set; }

		[Required]
		[StringLength(8)]
		public string OrderNumber { get; set; } = "";

		public int? CustomerId { get; set; }  // Yeh line change karo → int? bana do (nullable)

		[ForeignKey("CustomerId")]
		public Customer? Customer { get; set; }  // Yeh bhi nullable rehne do

		[Required]
		public string CustomerName { get; set; } = "";

		[Required]
		[EmailAddress]
		public string CustomerEmail { get; set; } = "";

		[Required]
		public string CustomerPhone { get; set; } = "";

		[Required]
		public string ShippingAddress { get; set; } = "";

		[Range(0, 1000)]
		public double Distance { get; set; } = 0;

		public decimal DeliveryCharges { get; set; } = 0;

		public string PaymentType { get; set; } = "COD";

		public string PaymentStatus { get; set; } = "Pending";

		public string OrderStatus { get; set; } = "Pending";

		public decimal ProductTotal { get; set; } = 0;

		public decimal TotalAmount { get; set; } = 0;

		public DateTime OrderDate { get; set; } = DateTime.Now;

		public DateTime? ShippedDate { get; set; }

		public DateTime? DeliveredDate { get; set; }

		public bool CanCancel { get; set; } = true;

		public DateTime? CancelledDate { get; set; }

		public decimal CancellationCharges { get; set; } = 0;

		public string Notes { get; set; } = "";

		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
	}
}