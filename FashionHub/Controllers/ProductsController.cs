using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionHub.Data;
using FashionHub.Models;

namespace FashionHub.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FashionHubContext _context;

        public ProductsController(FashionHubContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var fashionHubContext = _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Coupon);
            return View(await fashionHubContext.ToListAsync());
        }
        #region Sản phẩm đã bán
        public ActionResult HangTonKho()// Tổng số lượng đã bán
        {
           
                var data = _context.Products.GroupBy(p => new
                {
                    p.ProductName
                })
                        .Select(g => new HangTonKho
                        {
                            ProductName = g.Key.ProductName,
                            StockQuantity = g.Sum(ct => ct.StockQuantity),

                        }).ToList();
                return View(data);
        }
        public ActionResult HangTonKhoChart()
        {
            var data = _context.Products.GroupBy(p => new
            {
                p.ProductName
            })
                        .Select(g => new HangTonKho
                        {
                            ProductName = g.Key.ProductName,
                            StockQuantity = g.Sum(ct => ct.StockQuantity),

                        }).ToList();

            return Json(data);

        }
        #endregion Sản phẩm đã bán
        public IActionResult GetForm()
        {

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            // Thực hiện logic cần thiết để tạo form (ví dụ: load từ partial view)
            return PartialView("_AddProductFormPartial");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Coupon)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,BrandId,CategoryId,Description,Price,StockQuantity,Image,CouponId")] Product product, IFormFile ProductImage)
        {

            if (ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                // Kiểm tra xem OrderID đã được tạo chưa, nếu chưa thì tạo mới và lưu vào Session
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("CurrentOrderId")))
                {
                    HttpContext.Session.SetString("CurrentOrderId", Guid.NewGuid().ToString());
                }

                // Gán OrderID từ Session vào biến currentOrderId
                string currentOrderId = HttpContext.Session.GetString("CurrentOrderId");

                // Tạo một PurchaseOrder với OrderID hiện tại
                PurchaseOrder purchaseOrder = new PurchaseOrder
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = currentOrderId,
                    ProductId = Guid.NewGuid().ToString(),
                    Quantity = product.StockQuantity,
                    OrderDate = DateTime.Now,
                    BrandId = product.BrandId,
                };

                // Kiểm tra xem sản phẩm có tồn tại trong cơ sở dữ liệu không
                var existingProduct = _context.Products.FirstOrDefault(p => p.ProductName == product.ProductName);

                if (existingProduct != null)
                {
                    // Nếu sản phẩm đã tồn tại, cập nhật thông tin của nó
                    purchaseOrder.ProductId=existingProduct.ProductId;
                    existingProduct.Price = product.Price;
                    existingProduct.StockQuantity += product.StockQuantity;
                    existingProduct.Description = product.Description;
                    if (ProductImage != null)
                    {
                        existingProduct.Image = MyTool.UploadImageToFolder(ProductImage, "Products");
                    }
                }
                else
                {
                    // Nếu sản phẩm chưa tồn tại, gán OrderID và sử dụng ProductID đã tạo trước đó
                    product.ProductId = purchaseOrder.ProductId;
                    if (ProductImage != null)
                    {
                        product.Image = MyTool.UploadImageToFolder(ProductImage, "Products");
                    }

                    // Thêm sản phẩm vào cơ sở dữ liệu
                    _context.Add(product);
                }

                // Thêm PurchaseOrder vào cơ sở dữ liệu
                _context.Add(purchaseOrder);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Chuyển hướng đến trang Index hoặc nơi khác tùy thuộc vào logic của bạn
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", product.CouponId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", product.CouponId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,ProductName,BrandId,CategoryId,Description,Price,StockQuantity,Image,CouponId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandId", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["CouponId"] = new SelectList(_context.Coupons, "CouponId", "CouponId", product.CouponId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Coupon)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'FashionHubContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
