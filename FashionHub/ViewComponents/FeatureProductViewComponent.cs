using FashionHub.Data;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.ViewComponents
{

    public class FeatureProductViewComponent : ViewComponent
    {
        private readonly FashionHubContext db;

        public FeatureProductViewComponent(FashionHubContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.Products.Select(a => new ProductVM
            {
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                Price = a.Price,
                Image= a.Image,
            });
            return View(data);

        }
    }
}
