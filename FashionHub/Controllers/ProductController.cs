using Microsoft.AspNetCore.Mvc;

namespace FashionHub.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
