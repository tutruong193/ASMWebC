using ASMNhom3.Areas.Identity.Data;
using ASMNhom3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASMNhom3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ASMNhom3Context _db;
        public HomeController(ILogger<HomeController> logger, ASMNhom3Context context)
        {
            _logger = logger;
            _db = context;

        }

        public IActionResult Index()
        {
            var _product = getAllCategory();
            ViewBag.category = _product;
            return View();
        }
        public List<Category> getAllCategory()
        {
            return _db.Categorys.ToList();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}