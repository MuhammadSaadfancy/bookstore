using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly CrudDB _context;

		public CheckoutController(CrudDB context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			// Login check - agar user login nahi hai to login page pe bhejo
			if (HttpContext.Session.GetInt32("CustomerId") == null)
			{
				TempData["Error"] = "Please login to proceed to checkout.";
				return RedirectToAction("Login", "Customer", new { returnUrl = "/Checkout" });
			}

			// Agar login hai to cart items load karo
			var cartItems = _context.tbl_Cart
				.Include(c => c.Product)
				.Where(c => c.cart_status == 0)
				.ToList();

			if (!cartItems.Any())
			{
				return RedirectToAction("Index", "Cart");
			}

			ViewBag.CartItems = cartItems;
			ViewBag.TotalAmount = cartItems.Sum(item => item.product_quantity * decimal.Parse(item.Product?.product_price ?? "0"));

			return View();
		}

		[HttpPost]
		public IActionResult PlaceOrder(string customer_name, string customer_email, string customer_phone, string customer_address)
		{
			// Login check (double safety)
			var customerId = HttpContext.Session.GetInt32("CustomerId");
			if (customerId == null)
			{
				TempData["Error"] = "Please login to place order.";
				return RedirectToAction("Login", "Customer", new { returnUrl = "/Checkout" });
			}

			var cartItems = _context.tbl_Cart
				.Include(c => c.Product)
				.Where(c => c.cart_status == 0)
				.ToList();

			if (!cartItems.Any())
			{
				return RedirectToAction("Index", "Cart");
			}

			decimal total = cartItems.Sum(item => item.product_quantity * decimal.Parse(item.Product?.product_price ?? "0"));

			// Create Order
			var order = new Order
			{
				OrderNumber = CodeHelper.GenerateOrderNumber(_context),
				CustomerId = customerId.Value,  // Yeh line add karo (login se CustomerId set hoga)
				CustomerName = customer_name,
				CustomerEmail = customer_email,
				CustomerPhone = customer_phone,
				ShippingAddress = customer_address,
				ProductTotal = total,
				TotalAmount = total,
				OrderStatus = "Pending",
				OrderDate = DateTime.Now,
				PaymentType = "Cash on Delivery"
			};

			_context.tbl_order.Add(order);
			_context.SaveChanges();

			// Baqi code (OrderItems add karna) same rahega jo tumhare pass hai
			// ... (foreach loop for OrderItems same rahega)

			TempData["Success"] = $"Order placed successfully! Your Order Number: {order.OrderNumber}";
			return RedirectToAction("OrderSuccess", new { orderNumber = order.OrderNumber });
		}

		public IActionResult OrderSuccess(string orderNumber)
		{
			ViewBag.OrderNumber = orderNumber;
			return View();
		}
	}
}