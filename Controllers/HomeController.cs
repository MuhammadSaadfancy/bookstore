using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Controllers
{
	public class HomeController : Controller
	{
		private readonly CrudDB _context;

		public HomeController(CrudDB context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			// Featured products (latest 8 products)
			var featuredProducts = _context.tbl_Product
				.Include(p => p.Category)
				.OrderByDescending(p => p.CreatedDate)
				.Take(8)
				.ToList();

			ViewBag.Categories = _context.tbl_Category
				.OrderBy(c => c.Category_Name)
				.Take(6)
				.ToList();

			return View(featuredProducts);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
		}
	}
}