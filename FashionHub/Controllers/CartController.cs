using FashionHub.Data;
using FashionHub.Helper;
using FashionHub.Helpers;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FashionHub.Controllers
{
    public class CartController : Controller
    {
        private readonly FashionHubContext _context;
		 private readonly PaypalClient _paypalClient;

		public CartController(FashionHubContext context, PaypalClient paypalClient)
        {
			_paypalClient = paypalClient;
			_context = context;
        }

		
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

		public IActionResult Index()
		{
			return View(Cart);
		}
		[Authorize]
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

			HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

			return RedirectToAction("Index");
		}
		[Authorize]
		public IActionResult RemoveCart(string id)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.ProductId == id);
			if (item != null)
			{
				gioHang.Remove(item);
				HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
			}
			return RedirectToAction("Index");
		}
		[Authorize]
		[HttpGet]
		public IActionResult CheckOut()
		{
			if (Cart.Count == 0)
			{
				return Redirect("/");
			}
			ViewBag.PaypalClientId = _paypalClient.ClientId;
			return View(Cart);
		}

		[Authorize]
		[HttpPost]
		public IActionResult CheckOut(CheckOutVM model)
		{
            if (ModelState.IsValid)
            {
                var customerid = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                var user = new User();
                if (model.SameAddress)
                {
                    user = _context.Users.SingleOrDefault(kh => kh.UserId == customerid);
                }

                var order = new Order
                {
                    UserId = customerid,
					OrderId = Guid.NewGuid().ToString(),
					FullName = model.FullName ?? user.FullName,
                    Address = model.Address ?? user.Address,
                    PhoneNumber = model.PhoneNumber ?? user.PhoneNumber,
                    OrderDate = DateTime.Now,
                    PaymentMethod = "COD",
                    ShippingMethod = "Grab",
					ShipperId = 1,
					StatusId = 0,
                    Notes = model.Notes
                };

                _context.Database.BeginTransaction();
                try
                {
                    _context.Database.CommitTransaction();
                    _context.Add(order);
                    _context.SaveChanges();

                    var orderDetails = new List<OrderDetail>();
					string orderDetailId = Guid.NewGuid().ToString();
					foreach (var item in Cart)
                    {
                        orderDetails.Add(new OrderDetail
                        {
							OrderDetailId= orderDetailId,
							OrderId = order.OrderId,
                            Quantity = item.Quantity,
                            Subtotal = item.Price * item.Quantity,
                            ProductId = item.ProductId,
                            
                        });
                    }
					
                    _context.AddRange(orderDetails);
                    _context.SaveChanges();

                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Orders","Customer");
                }

                catch
                {
                    _context.Database.RollbackTransaction();
                }
            }

            return View(Cart);
		}
		[Authorize]
		[HttpPost("/Cart/PaypalOrder")]
		public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
		{
			// Thông tin đơn hàng gửi qua Paypal
			var tongTien = (Cart.Sum(p => p.SubTotal)+ (Cart.Sum(p => p.SubTotal) * 1 / 10)).ToString();
			var donViTienTe = "USD";
			var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);


				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		[Authorize]
		[HttpPost("/Cart/CapturePaypalOrder")]
		public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken, CheckOutVM model)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderID);

				var customerid = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
				var user = new User();
				if (model.SameAddress)
				{
					user = _context.Users.SingleOrDefault(kh => kh.UserId == customerid);
				}

				var order = new Order
				{
					UserId = customerid,
					OrderId = Guid.NewGuid().ToString(),
					FullName = model.FullName ?? user.FullName,
					Address = model.Address ?? user.Address,
					PhoneNumber = model.PhoneNumber ?? user.PhoneNumber,
					OrderDate = DateTime.Now,
					PaymentMethod = "PayPal",
					ShippingMethod = "Grab",
					ShipperId = 1,
					StatusId = 0,
					Notes = model.Notes
				};

				_context.Database.BeginTransaction();
				try
				{
					_context.Database.CommitTransaction();
					_context.Add(order);
					_context.SaveChanges();

					var orderDetails = new List<OrderDetail>();
					string orderDetailId1 = Guid.NewGuid().ToString();
					foreach (var item in Cart)
					{
						orderDetails.Add(new OrderDetail
						{
							OrderDetailId = orderDetailId1,
							OrderId = order.OrderId,
							Quantity = item.Quantity,
							Subtotal = item.Price * item.Quantity,
							ProductId = item.ProductId,

						});
					}

					_context.AddRange(orderDetails);
					_context.SaveChanges();

					HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

					
				}

				catch
				{
					_context.Database.RollbackTransaction();
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}
	}
}