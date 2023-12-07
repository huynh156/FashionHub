using FashionHub.Data;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.ViewComponents
{
    
    public class CategoriesViewComponent : ViewComponent
    {
        private  readonly FashionHubContext db;

        public CategoriesViewComponent(FashionHubContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(a => new CategoriesVM
            {
                CategoryId = a.CategoryId,
                CategoryName = a.CategoryName,
                StockQuantity = a.Products.Count
            });
            return View(data);

        }
    }
}
 