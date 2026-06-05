using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class CustomerController : Controller
	{
		private readonly CrudDB _context;

		public CustomerController(CrudDB context)
		{
			_context = context;
		}

		// سائن اپ (Register)
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(Customer customer)
		{
			// Remove client-side validation errors for optional fields (if any)
			ModelState.Remove("customer_gender");
			ModelState.Remove("customer_country");
			ModelState.Remove("customer_city");
			ModelState.Remove("customer_image");

			if (ModelState.IsValid)
			{
				// Check email unique
				if (_context.tbl_Customer.Any(c => c.customer_email == customer.customer_email))
				{
					ModelState.AddModelError("customer_email", "This email is already registered!");
					return View(customer);
				}

				// Default values for optional fields (taake DB mein null na jaye agar NOT NULL hai)
				customer.customer_gender = customer.customer_gender ?? "Not Specified";
				customer.customer_country = customer.customer_country ?? "Pakistan";
				customer.customer_city = customer.customer_city ?? "Karachi";
				customer.customer_image = customer.customer_image ?? "/images/default-user.jpg";

				try
				{
					_context.tbl_Customer.Add(customer);
					_context.SaveChanges();

					TempData["Success"] = "Account created successfully! Please login now.";
					return RedirectToAction("Login");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", "Error saving data: " + ex.Message);
				}
			}

			return View(customer);
		}

		// لاگ ان
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string customer_email, string customer_password, string returnUrl = null)
		{
			var customer = _context.tbl_Customer
				.FirstOrDefault(c => c.customer_email == customer_email && c.customer_password == customer_password);

			if (customer != null)
			{
				HttpContext.Session.SetInt32("CustomerId", customer.customer_id);
				HttpContext.Session.SetString("CustomerName", customer.customer_name);

				// Agar returnUrl hai to wahan redirect karo
				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return RedirectToAction("Index", "Home");
			}

			ViewBag.Error = "Invalid email or password!";
			return View();
		}

		// لاگ آؤٹ
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}