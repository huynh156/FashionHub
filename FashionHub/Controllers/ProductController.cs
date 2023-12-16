using FashionHub.Data;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.Controllers
{
	public class ProductController : Controller
	{
		private readonly FashionHubContext _context;

		public ProductController(FashionHubContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		// GET: Product/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Product/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ProductId,ProductName")] Product product)
		{
			if (ModelState.IsValid)
			{
				product.ProductId = Guid.NewGuid().ToString();
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}
	}
}
