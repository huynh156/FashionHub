using AutoMapper;
using FashionHub.Data;
using FashionHub.Helper;
using FashionHub.Models;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FashionHub.Controllers
{
	public class CustomerController : Controller
	{
		private readonly FashionHubContext db;
		private readonly IMapper _mapper;

		public IActionResult Index()
		{
			return View();
		}
		public CustomerController(FashionHubContext context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var khachHang = _mapper.Map<User>(model);
					khachHang.UserId = Guid.NewGuid().ToString();
					khachHang.RandomKey = MyTool.GenerateRamdomKey();
					khachHang.Password = model.Password.ToMd5Hash(khachHang.RandomKey);
					khachHang.IsActive = true;//sẽ xử lý khi dùng Mail để active
					khachHang.Role = "Customer";



					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Login", "Customer");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
			}
			return View();
		}
		[HttpGet]
		public IActionResult Login(string? ReturnURL)
		{
			ViewBag.ReturnURL = ReturnURL;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model, string? ReturnURL)
		{
			ViewBag.ReturnUrl = ReturnURL;
			if (ModelState.IsValid)
			{
				var khachHang = db.Users.SingleOrDefault(kh => kh.Username == model.UserName);
				if (khachHang == null)
				{
					ModelState.AddModelError("loi", "Không có khách hàng này");
				}
				else
				{
					if (khachHang.IsActive.HasValue && !khachHang.IsActive.Value)
					{
						ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
					}

					else
					{
						if (khachHang.Password != model.Password.ToMd5Hash(khachHang.RandomKey))
						{
							ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
						}
						else
						{
							var claims = new List<Claim> {
								new Claim(ClaimTypes.Email, khachHang.Email),
								new Claim(ClaimTypes.Name, khachHang.Username),
								new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.UserId),

								//claim - role động
								new Claim(ClaimTypes.Role, khachHang.Role)
							};

							var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
							var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

							await HttpContext.SignInAsync(claimsPrincipal);

							if (Url.IsLocalUrl(ReturnURL))
							{
								return Redirect(ReturnURL);
							}
							else
							{
								return RedirectToAction("MyAccount", "Customer");
							}
						}
					}
				}
			}
			return View();
		}
		[Authorize]
		public IActionResult MyAccount()
		{
			return View();
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{

			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
		[Authorize]
		public async Task<IActionResult> Orders()
		{
			var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
			var orders = await db.Orders
				.Include(p => p.OrderDetails)
				.Include(p => p.User)
				.Include(p => p.Status)
				.Where(m => m.UserId == customerId)
				.ToListAsync();

			return View(orders);
		}
		
	}
}
