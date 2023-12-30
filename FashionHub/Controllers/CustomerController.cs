using AutoMapper;
using FashionHub.Data;
using FashionHub.Helper;
using FashionHub.Models;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
					khachHang.UserId= Guid.NewGuid().ToString();
					khachHang.RandomKey = MyTool.GenerateRamdomKey();
					khachHang.Password = model.Password.ToMd5Hash(khachHang.RandomKey);
					khachHang.IsActive = true;//sẽ xử lý khi dùng Mail để active
					khachHang.Role = "Customer";

					

					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Index", "Product");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
			}
			return View();
		}

	}
}
