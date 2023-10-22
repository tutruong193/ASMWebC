using ASMNhom3.Areas.Identity.Data;
using ASMNhom3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace ASMNhom3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ASMNhom3Context _db;
        private readonly UserManager<ManageUser> _userManager;
        public HomeController(ILogger<HomeController> logger, ASMNhom3Context context, UserManager<ManageUser> userManager)
        {
            _logger = logger;
            _db = context;
            _userManager = userManager;
        }
        //thao tac voi cart
        public IActionResult addCart(int id, int quanlity)
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập
            if (user != null)
            {
                var product = getDetailBook(id);
                var existingCartItem = _db.Carts.FirstOrDefault(c => c.EmailUser == user.Email && c.BookID == product.BookID);

                if (existingCartItem != null)
                {
                    existingCartItem.Quanlity += quanlity;
                }
                else
                {
                    var newCartItem = new Cart
                    {
                        EmailUser = user.Email,
                        BookID = product.BookID,
                        Quanlity = quanlity,
                        Total = product.Price
                    };
                    _db.Carts.Add(newCartItem);
                }
                product.Quantity -= quanlity;
                _db.SaveChanges(); // Lưu dữ liệu vào cơ sở dữ liệu

                return RedirectToAction("Details", "Home", new { id = product.BookID });
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult deleteCart(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var cart = getCart(id);
                if (cart != null)
                {
                    var product = getDetailBook(cart.BookID);
                    if (product != null)
                    {
                        product.Quantity += cart.Quanlity;
                        _db.Carts.Remove(cart);
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ListCart", "Home");
        }
        public IActionResult ListCart()
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập

            if (user != null)
            {
                var userCarts = _db.Carts
                    .Where(c => c.EmailUser == user.Email)
                    .Include(c => c.Book) // Include thông tin của Book từ BookID
                    .ToList();

                var total = userCarts.Sum(item => item.Book.Price * item.Quanlity);

                ViewBag.userCarts = userCarts;
                ViewBag.TotalPrice = total;

                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ChecKOut()
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập
            if (user != null)
            {
                //ViewBag.product = _db.Carts.Where(c => c.EmailUser == user.Email).Select(c => c.BookID).ToList();
                List<Cart> carts = new List<Cart>();
                ViewBag.TotalPrice = 0;
                ViewBag.TotalQuanlity = 0;
                foreach (var cart in _db.Carts)
                {
                    if (user.Email == cart.EmailUser)
                    {
                        carts.Add(cart);
                    }
                }
                foreach (var cart in _db.Carts)
                {
                    foreach (var book in _db.Books)
                    {
                        if (user.Email == cart.EmailUser && cart.BookID == book.BookID)
                        {
                            ViewBag.TotalPrice += cart.Quanlity * book.Price;
                            ViewBag.TotalQuanlity += cart.Quanlity;
                        }
                    }
                }
                ViewBag.book = getAllBook();
                ViewBag.product = carts;
                ViewBag.user = _db.Users.FirstOrDefault(c => c.Email == user.Email);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ConfirmCheckOut(QueueCheckOut model)
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập
            if (ModelState.IsValid)
            {
                var newQueue = new QueueCheckOut
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNum = model.PhoneNum,
                    Address = model.Address,
                    Email = model.Email,
                    Note = model.Note,
                    TotalPrice = model.TotalPrice,
                    TotalQuantity = model.TotalQuantity,
                    IsConfirm = false
                };
                _db.QueueCheckOuts.Add(newQueue);
                List<Cart> oldcart = new List<Cart>();
                foreach(var cart in _db.Carts)
                {
                    if(cart.EmailUser == user.Email)
                    {
                        oldcart.Add(cart);
                    }
                }
                for(int i = 0; i < oldcart.Count(); i++)
                {
                    _db.Carts.Remove(oldcart[i]);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("ChecKOut");
        }
        //hienthibook
        public IActionResult Index()
        {
            var category = getAllCategory();
            ViewBag.category = category;
            return View();
        }
        public IActionResult ListBook()
        {
            var _product = getAllBook();
            ViewBag.book = _product;
            return View();
        }
        public IActionResult Details(int id)
        {
            var book = _db.Books.Find(id);
            return View(book);
        }
        //hamphu
        public List<Book> getAllBook()
        {
            return _db.Books.ToList();
        }
        private List<Category> getAllCategory()
        {
            return _db.Categorys.ToList();
        }
        private Book getDetailBook(int id)
        {
            return _db.Books.Find(id);
        }
        private Cart getCart(int id)
        {
            return _db.Carts.Find(id);
        }
        //

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