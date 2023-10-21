using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASMNhom3.Areas.Identity.Data;
using ASMNhom3.Models;

namespace ASMNhom3.Controllers
{
    public class BooksController : Controller
    {
        private readonly ASMNhom3Context _db;

        public BooksController(ASMNhom3Context context)
        {
            _db = context;
        }
        public ActionResult Index()
        {

            var _product = getAllProduct();
            ViewBag.book = _product;
            return View();
        }

        //GET ALL PRODUCT
        public List<Book> getAllProduct()
        {   
            return _db.Books.ToList();
        }
        //GET DETAIL PRODUCT
        public ActionResult Details(int id)
        {
            var book = _db.Books.Find(id);
            return View(book);
        }
    }
}
