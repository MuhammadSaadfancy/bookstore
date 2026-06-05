using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class FeedbackController : Controller
	{
		private readonly CrudDB _context;

		public FeedbackController(CrudDB context)
		{
			_context = context;
		}

		// Feedback Form Display
		public IActionResult Index()
		{
			return View();
		}

		// Submit Feedback
		[HttpPost]
		public IActionResult SubmitFeedback(Feedback feedback)
		{
			if (ModelState.IsValid)
			{
				// Customer session se name lena agar login hai
				var customerName = HttpContext.Session.GetString("CustomerName");
				feedback.user_name = !string.IsNullOrEmpty(customerName) ? customerName : "Guest";

				_context.tbl_Feedback.Add(feedback);
				_context.SaveChanges();

				TempData["Success"] = "Thank you for your feedback!";
				return RedirectToAction("Index", "Home");
			}

			return View("Index", feedback);
		}
	}
}