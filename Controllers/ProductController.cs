using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Controllers
{
	public class ProductsController : Controller
	{
		private readonly CrudDB _context;

		public ProductsController(CrudDB context)
		{
			_context = context;
		}

		// All Products List
		public IActionResult Index()
		{
			var products = _context.tbl_Product
				.Include(p => p.Category)
				.OrderByDescending(p => p.CreatedDate)
				.ToList();

			return View(products);
		}

		// Product Details
		public IActionResult Details(int id)
		{
			var product = _context.tbl_Product
				.Include(p => p.Category)
				.FirstOrDefault(p => p.product_id == id);

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
	}
}