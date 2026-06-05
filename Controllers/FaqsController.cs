using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
	public class FAQController : Controller
	{
		private readonly CrudDB _context;

		public FAQController(CrudDB context)
		{
			_context = context;
		}

		// FAQ Page Display
		public IActionResult Index()
		{
			var faqs = _context.tbl_Faqs
				.OrderBy(f => f.faqs_Id)
				.ToList();

			return View(faqs);
		}
	}
}