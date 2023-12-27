using FashionHub.Data;
using FashionHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionHub.ViewComponents
{

    public class BrandsViewComponent : ViewComponent
    {
        private readonly FashionHubContext db;

        public BrandsViewComponent(FashionHubContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.Brands.Select(a => new BrandsVM
            {
                BrandId = a.BrandId,
                BrandName = a.BrandName
            }) ;
            return View(data);

        }
    }
}
