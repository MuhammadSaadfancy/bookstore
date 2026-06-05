using System.Linq;
using BookStore.Models;
using BookStore.Models;

namespace BookStore.Helpers
{
	public static class CodeHelper
	{
		// Generate 7-digit product code: CAT-XXXX
		public static string GenerateProductCode(CrudDB context, int categoryId)
		{
			var category = context.tbl_Category.Find(categoryId);
			string catCode = GetCategoryCode(category?.Category_Name ?? "GN");

			int lastId = context.tbl_Product
				.Where(p => p.cat_id == categoryId)
				.OrderByDescending(p => p.product_id)
				.Select(p => p.product_id)
				.FirstOrDefault();

			int seq = lastId + 1;
			return $"{catCode}-{seq.ToString("D5")}";
		}

		// Generate 8-digit order number: YYMMDDNN
		public static string GenerateOrderNumber(CrudDB context)
		{
			string date = DateTime.Now.ToString("yyMMdd");

			int todayCount = context.tbl_order
				.Where(o => o.OrderDate.Date == DateTime.Today)
				.Count();

			int seq = todayCount + 1;
			return $"{date}{seq.ToString("D2")}";
		}

		private static string GetCategoryCode(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName)) return "GN";
			if (categoryName.Length >= 2) return categoryName.Substring(0, 2).ToUpper();
			return categoryName.ToUpper().PadRight(2, 'X');
		}

		// Calculate delivery charges
		public static decimal CalculateDelivery(double distance, decimal orderAmount)
		{
			if (distance <= 3) return 0;

			decimal charge = 50 + ((decimal)distance - 3) * 10;
			if (orderAmount > 500) charge *= 0.9m;
			if (orderAmount > 1000) charge *= 0.8m;

			return Math.Round(charge, 2);
		}
	}
}