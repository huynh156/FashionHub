using FashionHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    OrderId = Guid.NewGuid().ToString(),
                    // Copy các trường khác tùy thuộc vào yêu cầu của bạn
                    ProductId = product.ProductId, // Copy ProductId hoặc trường khác từ Product
                    Quantity = product.StockQuantity // Copy ProductName hoặc trường khác từ Product
                };


                product.ProductId = Guid.NewGuid().ToString();
				var old_StockQuantity = _context.Products.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
                product.StockQuantity = old_StockQuantity.StockQuantity + product.StockQuantity;
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}
	}
}
