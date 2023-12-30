using FashionHub.Data;
using FashionHub.Helpers;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.Controllers
{
    public class CartController : Controller
    {
        private readonly FashionHubContext _context;

        public CartController(FashionHubContext context)
        {
            _context = context;
        }

		const string CART_KEY = "MYCART";
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

		public IActionResult Index()
		{
			return View(Cart);
		}

		public IActionResult AddToCart(string id, int quantity = 1)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.ProductId == id);
			if (item == null)
			{
				var hangHoa = _context.Products.SingleOrDefault(p => p.ProductId == id);
				if (hangHoa == null)
				{
					TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
					return Redirect("/404");
				}
				item = new CartItem
				{
					ProductId = hangHoa.ProductId,
					ProductName = hangHoa.ProductName,
					Price = hangHoa.Price ?? 0,
					Image = hangHoa.Image ?? string.Empty,
					Quantity = quantity,
                    SubTotal  = hangHoa.Price.HasValue ? hangHoa.Price.Value * quantity : 0
				};
				gioHang.Add(item);
			}
			else
			{
				item.Quantity += quantity;
			}

			HttpContext.Session.Set(CART_KEY, gioHang);

			return RedirectToAction("Index");
		}

		public IActionResult RemoveCart(string id)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.ProductId == id);
			if (item != null)
			{
				gioHang.Remove(item);
				HttpContext.Session.Set(CART_KEY, gioHang);
			}
			return RedirectToAction("Index");
		}
	}
}
