using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BooksController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public ActionResult Index()
        {

            var _product = getAllProduct();
            ViewBag.book= _product;
            return View();
        }

        //GET ALL PRODUCT
        public List<Book> getAllProduct()
        {
            return _db.Book.ToList();
        }
    }
}
