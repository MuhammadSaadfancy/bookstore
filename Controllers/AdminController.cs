using System.Linq;
using BookStore.Helpers;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShradhaBookStore8.Controllers
{
	public class AdminController : Controller
	{
		private readonly CrudDB _context;
		private readonly IWebHostEnvironment _environment;

		public AdminController(CrudDB context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		// ================ LOGIN ================
		public IActionResult Login()
		{
			if (HttpContext.Session.GetInt32("AdminId") != null)
				return RedirectToAction("Dashboard");
			return View();
		}

		[HttpPost]
		public IActionResult Login(string Admin_Email, string Admin_Password)
		{
			var admin = _context.tbl_Admin
				.FirstOrDefault(a => a.Admin_Email == Admin_Email && a.Admin_Password == Admin_Password);

			if (admin != null)
			{
				HttpContext.Session.SetInt32("AdminId", admin.Admin_Id);
				HttpContext.Session.SetString("AdminName", admin.Admin_Name);
				return RedirectToAction("Dashboard");
			}

			ViewBag.Error = "Invalid email or password!";
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}

		// ================ DASHBOARD ================
		public IActionResult Dashboard()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			ViewBag.TotalProducts = _context.tbl_Product.Count();
			ViewBag.TotalCustomers = _context.tbl_Customer.Count();
			ViewBag.TotalOrders = _context.tbl_order.Count();
			ViewBag.TotalCategories = _context.tbl_Category.Count();
			ViewBag.PendingOrders = _context.tbl_order.Count(o => o.OrderStatus == "Pending");
			ViewBag.TodayOrders = _context.tbl_order.Count(o => o.OrderDate.Date == DateTime.Today);

			// Recent orders
			var recentOrders = _context.tbl_order
				.Include(o => o.OrderItems)
				.OrderByDescending(o => o.OrderDate)
				.Take(5)
				.ToList();

			return View(recentOrders);
		}

		// ================ PRODUCT MANAGEMENT ================
		public IActionResult Products()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var products = _context.tbl_Product
				.Include(p => p.Category)
				.OrderByDescending(p => p.product_id)
				.ToList();

			ViewBag.Categories = _context.tbl_Category.ToList();
			return View(products);
		}

		public IActionResult AddProduct()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			ViewBag.Categories = _context.tbl_Category.ToList();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct(Product product, IFormFile? productImage)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			if (ModelState.IsValid)
			{
				// Generate product code
				product.ProductCode = CodeHelper.GenerateProductCode(_context, product.cat_id);

				// Handle image upload
				if (productImage != null && productImage.Length > 0)
				{
					var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
					if (!Directory.Exists(uploadsFolder))
						Directory.CreateDirectory(uploadsFolder);

					var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await productImage.CopyToAsync(stream);
					}

					product.product_image = "/uploads/" + uniqueFileName;
				}

				product.CreatedDate = DateTime.Now;
				_context.tbl_Product.Add(product);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Product added successfully!";
				return RedirectToAction("Products");
			}

			ViewBag.Categories = _context.tbl_Category.ToList();
			return View(product);
		}

		public IActionResult EditProduct(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var product = _context.tbl_Product.Find(id);
			if (product == null)
			{
				TempData["Error"] = "Product not found!";
				return RedirectToAction("Products");
			}

			ViewBag.Categories = _context.tbl_Category.ToList();
			return View(product);
		}

		[HttpPost]
		public async Task<IActionResult> EditProduct(Product product, IFormFile? productImage)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			if (ModelState.IsValid)
			{
				// Handle image upload
				if (productImage != null && productImage.Length > 0)
				{
					var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
					var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await productImage.CopyToAsync(stream);
					}

					product.product_image = "/uploads/" + uniqueFileName;
				}

				product.UpdatedDate = DateTime.Now;
				_context.tbl_Product.Update(product);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Product updated successfully!";
				return RedirectToAction("Products");
			}

			ViewBag.Categories = _context.tbl_Category.ToList();
			return View(product);
		}

		public IActionResult DeleteProduct(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var product = _context.tbl_Product.Find(id);
			if (product != null)
			{
				_context.tbl_Product.Remove(product);
				_context.SaveChanges();
				TempData["Success"] = "Product deleted successfully!";
			}

			return RedirectToAction("Products");
		}

		// ================ CATEGORY MANAGEMENT ================
		public IActionResult Categories()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var categories = _context.tbl_Category.OrderBy(c => c.Category_Name).ToList();
			return View(categories);
		}

		[HttpPost]
		public IActionResult AddCategory(string Category_Name)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			if (!string.IsNullOrEmpty(Category_Name))
			{
				var category = new Category { Category_Name = Category_Name };
				_context.tbl_Category.Add(category);
				_context.SaveChanges();
				TempData["Success"] = "Category added successfully!";
			}

			return RedirectToAction("Categories");
		}

		public IActionResult DeleteCategory(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var category = _context.tbl_Category.Find(id);
			if (category != null)
			{
				// Check if category has products
				var hasProducts = _context.tbl_Product.Any(p => p.cat_id == id);
				if (hasProducts)
				{
					TempData["Error"] = "Cannot delete category with products!";
				}
				else
				{
					_context.tbl_Category.Remove(category);
					_context.SaveChanges();
					TempData["Success"] = "Category deleted successfully!";
				}
			}

			return RedirectToAction("Categories");
		}

		// ================ CUSTOMER MANAGEMENT ================
		public IActionResult Customers()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var customers = _context.tbl_Customer
				.OrderByDescending(c => c.customer_id)
				.ToList();

			return View(customers);
		}

		// ================ ORDER MANAGEMENT ================
		public IActionResult Orders()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var orders = _context.tbl_order
				.Include(o => o.OrderItems)
				.OrderByDescending(o => o.OrderDate)
				.ToList();

			return View(orders);
		}

		public IActionResult OrderDetails(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var order = _context.tbl_order
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.Product)
				.FirstOrDefault(o => o.OrderId == id);

			if (order == null)
			{
				TempData["Error"] = "Order not found!";
				return RedirectToAction("Orders");
			}

			return View(order);
		}

		// ========== IMPORTANT FIX: ADD GET METHOD FOR BUTTONS ==========
		[HttpGet]
		public IActionResult UpdateOrderStatus(int orderId, string status)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var order = _context.tbl_order.Find(orderId);
			if (order != null)
			{
				string oldStatus = order.OrderStatus;
				order.OrderStatus = status;

				if (status == "Processing")
				{
					// Processing ke liye kuch special nahi
				}
				else if (status == "Shipped")
				{
					order.ShippedDate = DateTime.Now;
				}
				else if (status == "Delivered")
				{
					order.DeliveredDate = DateTime.Now;
					order.CanCancel = false;
				}
				else if (status == "Cancelled")
				{
					order.CancelledDate = DateTime.Now;
					order.CanCancel = false;
				}

				_context.SaveChanges();
				TempData["Success"] = $"Order #{order.OrderNumber} status updated to {status}";

				return RedirectToAction("OrderDetails", new { id = orderId });
			}

			TempData["Error"] = "Order not found!";
			return RedirectToAction("Orders");
		}

		[HttpPost]
		public IActionResult UpdateOrderStatusPost(int orderId, string status)
		{
			// Yeh POST method backup ke liye
			return UpdateOrderStatus(orderId, status);
		}

		[HttpGet]
		public IActionResult CancelOrder(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var order = _context.tbl_order.Find(id);
			if (order != null && order.CanCancel)
			{
				order.OrderStatus = "Cancelled";
				order.CancelledDate = DateTime.Now;
				order.CanCancel = false;
				_context.SaveChanges();
				TempData["Success"] = $"Order #{order.OrderNumber} has been cancelled.";
			}
			else
			{
				TempData["Error"] = "Order cannot be cancelled.";
			}

			return RedirectToAction("OrderDetails", new { id = id });
		}

		// Order notes update ke liye
		[HttpPost]
		public IActionResult UpdateOrderNotes(int orderId, string notes)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var order = _context.tbl_order.Find(orderId);
			if (order != null)
			{
				order.Notes = notes;
				_context.SaveChanges();
				TempData["Success"] = "Order notes updated!";
			}

			return RedirectToAction("OrderDetails", new { id = orderId });
		}

		// ================ FAQ MANAGEMENT ================
		public IActionResult FAQs()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var faqs = _context.tbl_Faqs.OrderBy(f => f.faqs_Id).ToList();
			return View(faqs);
		}

		[HttpPost]
		public IActionResult AddFAQ(string faqs_Question, string faqs_Answer)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			if (!string.IsNullOrEmpty(faqs_Question) && !string.IsNullOrEmpty(faqs_Answer))
			{
				var faq = new Faqs
				{
					faqs_Question = faqs_Question,
					faqs_Answer = faqs_Answer
				};
				_context.tbl_Faqs.Add(faq);
				_context.SaveChanges();
				TempData["Success"] = "FAQ added successfully!";
			}

			return RedirectToAction("FAQs");
		}

		public IActionResult DeleteFAQ(int id)
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var faq = _context.tbl_Faqs.Find(id);
			if (faq != null)
			{
				_context.tbl_Faqs.Remove(faq);
				_context.SaveChanges();
				TempData["Success"] = "FAQ deleted successfully!";
			}

			return RedirectToAction("FAQs");
		}

		// ================ FEEDBACK MANAGEMENT ================
		public IActionResult Feedback()
		{
			if (HttpContext.Session.GetInt32("AdminId") == null)
				return RedirectToAction("Login");

			var feedbacks = _context.tbl_Feedback
				.OrderByDescending(f => f.feedback_Id)
				.ToList();

			return View(feedbacks);
		}
	}
}