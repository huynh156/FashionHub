using FashionHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionHub.Controllers
{
    public class CouponsController : Controller
    {
        private readonly FashionHubContext _context;

        public CouponsController(FashionHubContext context)
        {
            _context = context;
        }

        // GET: Coupons
        public async Task<IActionResult> Index()
        {
            var fashionHubContext = _context.Coupons.Include(c => c.Product);
            return View(await fashionHubContext.ToListAsync());
        }

        // GET: Coupons/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CouponId == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // GET: Coupons/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponId,Code,DiscountPercentage,ExpiryDate,IsActive,Quantity,ProductId")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                coupon.IsActive = true;
                _context.Add(coupon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", coupon.ProductId);
            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", coupon.ProductId);
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CouponId,Code,DiscountPercentage,ExpiryDate,IsActive,Quantity,ProductId")] Coupon coupon)
        {
            if (id != coupon.CouponId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.CouponId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", coupon.ProductId);
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CouponId == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Coupons == null)
            {
                return Problem("Entity set 'FashionHubContext.Coupons'  is null.");
            }
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouponExists(string id)
        {
            return (_context.Coupons?.Any(e => e.CouponId == id)).GetValueOrDefault();
        }
    }
}
