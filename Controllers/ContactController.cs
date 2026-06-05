using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class ContactController : Controller
	{
		private readonly CrudDB _context;

		public ContactController(CrudDB context)
		{
			_context = context;
		}

		// GET: /Contact
		public IActionResult Index()
		{
			return View();
		}

		// POST: /Contact/SendMessage
		[HttpPost]
		public IActionResult SendMessage(string name, string email, string subject, string message)
		{
			try
			{
				// 1. VALIDATION
				if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
					string.IsNullOrEmpty(message))
				{
					TempData["Error"] = "Please fill all required fields!";
					return RedirectToAction("Index");
				}

				// 2. SAVE TO FEEDBACK TABLE
				var feedback = new Feedback
				{
					user_name = $"{name} | {email} | {subject}",
					user_message = $"📧 CONTACT FORM MESSAGE\n" +
								  $"📅 Date: {DateTime.Now:dd MMM yyyy HH:mm}\n" +
								  $"👤 Name: {name}\n" +
								  $"📩 Email: {email}\n" +
								  $"📌 Subject: {subject}\n" +
								  $"📝 Message:\n{message}\n" +
								  $"────────────────────────────"
				};

				_context.tbl_Feedback.Add(feedback);
				_context.SaveChanges();

				// 3. SUCCESS MESSAGE
				TempData["Success"] = $"Thank you <strong>{name}</strong>! ✅<br>" +
									 "Your message has been received successfully.<br>" +
									 "We will contact you soon.";

				// 4. LOG FOR DEBUGGING
				Console.WriteLine($"✅ Contact Form Submitted:");
				Console.WriteLine($"   Name: {name}");
				Console.WriteLine($"   Email: {email}");
				Console.WriteLine($"   Subject: {subject}");
				Console.WriteLine($"   Message: {message.Substring(0, Math.Min(50, message.Length))}...");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// 5. ERROR HANDLING
				TempData["Error"] = "Sorry, there was an error sending your message. Please try again.";
				Console.WriteLine($"❌ Contact Form Error: {ex.Message}");
				return RedirectToAction("Index");
			}
		}
	}
}