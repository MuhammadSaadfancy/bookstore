using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
	public class Cart
	{
		[Key]
		public int cart_id { get; set; }

		public int prod_id { get; set; }

		[ForeignKey("prod_id")]
		public Product? Product { get; set; }  // Yeh line add karo (important!)

		public int cust_id { get; set; }

		public int product_quantity { get; set; }

		// Cart status: 0=In Cart, 1=Ordered, 2=Cancelled
		public int cart_status { get; set; } = 0;

		public DateTime AddedDate { get; set; } = DateTime.Now;
	}
}