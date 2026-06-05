using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;
using BookStore.Models;

namespace BookStore.Models
{
	public class Product
	{
		[Key]
		public int product_id { get; set; }

		[Required(ErrorMessage = "Product name is required")]
		public string product_name { get; set; } = "";

		[Required(ErrorMessage = "Price is required")]
		public string product_price { get; set; } = "";

		public string product_description { get; set; } = "";
		public string product_image { get; set; } = "";

		[Required(ErrorMessage = "Category is required")]
		public int cat_id { get; set; }

		[ForeignKey("cat_id")]
		public Category? Category { get; set; }

		[Range(0, 10000, ErrorMessage = "Stock quantity must be between 0 and 10000")]
		public int StockQuantity { get; set; } = 0;

		public string Author { get; set; } = "";
		public string Publisher { get; set; } = "";
		public DateTime? ReleaseDate { get; set; }
		public string Version { get; set; } = "";
		public bool IsActive { get; set; } = true;

		// 7-digit product code
		public string ProductCode { get; set; } = "";

		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public DateTime? UpdatedDate { get; set; }
	}
}