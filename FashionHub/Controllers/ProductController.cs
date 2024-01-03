using FashionHub.Data;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace FashionHub.Controllers
{
    public class ProductController : Controller
    {
        private readonly FashionHubContext _context;

        public ProductController(FashionHubContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? categoryId)
        {

            var data = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(categoryId))
            {
                data = data.Where(p => p.CategoryId == categoryId);
            }
            var result = data.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                SlugName = p.SlugName,
                Price = p.Price,
                Image = p.Image,
                Description = p.Description,
                CategoryId = p.Category.CategoryName

            }); ;
            return View(result);
        }
        //var data = _context.Products.AsQueryable();

        //if (!string.IsNullOrEmpty(categoryId) && !string.IsNullOrEmpty(brandId))
        //{
        //    // OR condition between categoryId and brandId
        //    data = data.Where(p => p.CategoryId == categoryId || p.BrandId == brandId);
        //}
        //else if (!string.IsNullOrEmpty(categoryId))
        //{
        //    // Only categoryId condition
        //    data = data.Where(p => p.CategoryId == categoryId);
        //}
        //else if (!string.IsNullOrEmpty(brandId))
        //{
        //    // Only brandId condition
        //    data = data.Where(p => p.BrandId == brandId);
        //}

        //var end_data = data.ToList();

        [HttpGet("/Products/{tensp}")]
        public async Task<IActionResult> Details(string tensp)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.SlugName == tensp);
            if (product == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm {product.ProductName}";
                return NotFound();
            }
            var data = new ProductVM
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                SlugName= product.SlugName,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Image = product.Image,
                Description = product.Description,

            };
            string currentUrl = HttpContext.Request.GetEncodedUrl();
            ViewData["CurrentUrl"] = currentUrl;


            return View(data);
        }
        public IActionResult FeatureProduct()
        {
            var product = _context.Products.Take(4).ToList();
            var result = product.Select(p => new FashionHub.ViewModels.ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Image = p.Image,
                Description = p.Description,
                CategoryId = p.Category.CategoryName
            }).ToList();

            return PartialView("_FeatureProduct", result);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var data = _context.Products.Include(a => a.Brand).Where(a => a.ProductName.Contains(keyword)).ToList();
            var result = data.Select(p => new SearchResultVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                BrandName = p.Brand.BrandName,
                Price = p.Price ?? 0,
                Image = p.Image,

            });

            return PartialView("SearchResult", result);
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
