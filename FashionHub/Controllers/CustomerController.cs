using FashionHub.Data;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.Controllers
{
	public class CustomerController : Controller
	{
		private readonly FashionHubContext db;

		public IActionResult Index()
		{
			return View();
		}
		public CustomerController(FashionHubContext context)
		{
			db = context;
		}
		public IActionResult Register()
		{
			return View();
		}

	}
}
