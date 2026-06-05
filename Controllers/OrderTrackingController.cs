using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Controllers
{
	public class OrderTrackingController : Controller
	{
		private readonly CrudDB _context;

		public OrderTrackingController(CrudDB context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Track(string orderNumber)
		{
			var order = _context.tbl_order
				.Include(o => o.OrderItems)  // Yeh zaroori hai
				.FirstOrDefault(o => o.OrderNumber == orderNumber);

			// Debug ke liye
			if (order != null)
			{
				Console.WriteLine($"Order Found: {order.OrderNumber}");
				Console.WriteLine($"OrderItems Count: {order.OrderItems?.Count}");

				if (order.OrderItems != null)
				{
					foreach (var item in order.OrderItems)
					{
						Console.WriteLine($"Item: {item.ProductName}, Qty: {item.Quantity}, Price: {item.Price}");
					}
				}
			}

			return View(order);
		}
	}
}