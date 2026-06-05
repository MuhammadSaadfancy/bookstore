using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class CartController : Controller
	{
		private readonly CrudDB _context;

		public CartController(CrudDB context)
		{
			_context = context;
		}

		// View Cart
		public IActionResult Index()
		{
			var cartItems = _context.tbl_Cart
				.Include(c => c.Product)
				.Where(c => c.cart_status == 0)
				.ToList();

			return View(cartItems);
		}

		// Add to Cart
		[HttpPost]
		public IActionResult AddToCart(int prod_id, int quantity = 1)
		{
			var existing = _context.tbl_Cart
				.FirstOrDefault(c => c.prod_id == prod_id && c.cart_status == 0);

			if (existing != null)
			{
				existing.product_quantity += quantity;
			}
			else
			{
				var cartItem = new Cart
				{
					prod_id = prod_id,
					product_quantity = quantity,
					cart_status = 0,
					AddedDate = DateTime.Now
				};
				_context.tbl_Cart.Add(cartItem);
			}

			_context.SaveChanges();
			TempData["Success"] = "Item added to cart!";
			return RedirectToAction("Index");
		}

		// Update Quantity (+ or - button)
		[HttpPost]
		public IActionResult UpdateQuantity(int id, int quantity)
		{
			var item = _context.tbl_Cart.Find(id);
			if (item != null && quantity > 0)
			{
				item.product_quantity = quantity;
				_context.SaveChanges();
			}
			else if (item != null && quantity <= 0)
			{
				_context.tbl_Cart.Remove(item); // agar quantity 0 ho jaye to remove
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		// Remove from Cart
		public IActionResult Remove(int id)
		{
			var item = _context.tbl_Cart.Find(id);
			if (item != null)
			{
				_context.tbl_Cart.Remove(item);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}
	}
}