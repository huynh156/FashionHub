using FashionHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FashionHub.Models;

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
            var data = _context.Products.ToList();
            return View(data);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var data = _context.Products.Include(a => a.Brand).Where(a => a.ProductName.Contains(keyword)).ToList();

            return PartialView("PartialViewProduct", data);
        }

        [HttpPost]
        public IActionResult AdvancedSearch(string keyword, int? fromValue, int? toValue)
        {
            if (fromValue == null)
            {
                fromValue = 0;
            }
            if (toValue == null)
            {
                toValue = 999999999;
            }
            if (keyword == null)
            {
                var data = _context.Products.Include(a => a.Brand).Where(a => a.StockQuantity > 1 && a.Price >= fromValue && a.Price <= toValue).ToList();
                return PartialView("PartialViewProduct", data);
            }
            else
            {
                var data = _context.Products.Include(a => a.Brand).Where(a => a.StockQuantity > 1 && a.ProductName.Contains(keyword) && a.Price >= fromValue && a.Price <= toValue).ToList();
                return PartialView("PartialViewProduct", data);
            }


        }
    }
}
