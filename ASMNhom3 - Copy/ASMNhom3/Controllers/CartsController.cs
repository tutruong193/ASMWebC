using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASMNhom3.Areas.Identity.Data;
using ASMNhom3.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ASMNhom3.Controllers
{
    public class CartsController : Controller
    {
        private readonly ILogger<CartsController> _logger;
        private readonly ASMNhom3Context _db;
        private readonly UserManager<ManageUser> _userManager;


        public CartsController(ILogger<CartsController> logger, ASMNhom3Context context, UserManager<ManageUser> userManager)
        {
            _logger = logger;
            _db = context;
            _userManager = userManager;
        }
        public IActionResult addCart(int id, int quanlity)
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập
            if (user != null)
            {
                var product = getDetailProduct(id);
                var existingCartItem = _db.Carts.FirstOrDefault(c => c.EmailUser == user.Email && c.BookID == product.BookID);

                if (existingCartItem != null)
                {
                    existingCartItem.Quanlity+=quanlity;
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

                return RedirectToAction("Details", "Books", new { id = product.BookID });
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
                    var product = getDetailProduct(cart.BookID);
                    if (product != null)
                    {
                        product.Quantity += cart.Quanlity;
                        _db.Carts.Remove(cart);
                        _db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("CartList", "Carts");
        }

        private Book getDetailProduct(int id)
        {
            return _db.Books.Find(id);
        }
        private Cart getCart(int id)
        {
            return _db.Carts.Find(id);
        }
        public IActionResult CartList()
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



        public IActionResult ChechOut()
        {
            var user = _userManager.GetUserAsync(User).Result; // Lấy thông tin người dùng đăng nhập
            if (user != null)
            {
                ViewBag.product = _db.Carts.Where(c => c.EmailUser == user.Email).Include(c => c.BookID).ToList();
                ViewBag.user = _db.Users.FirstOrDefault(c => c.Email == user.Email);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }


        //// GET: Carts
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Carts != null ? 
        //                  View(await _context.Carts.ToListAsync()) :
        //                  Problem("Entity set 'ASMNhom3Context.Carts'  is null.");
        //}

        //// GET: Carts/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts
        //        .FirstOrDefaultAsync(m => m.CartID == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        //// GET: Carts/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Carts/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CartID,EmailUser,Quanlity,Total")] Cart cart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(cart);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        //// GET: Carts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts.FindAsync(id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(cart);
        //}

        //// POST: Carts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CartID,EmailUser,Quanlity,Total")] Cart cart)
        //{
        //    if (id != cart.CartID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(cart);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CartExists(cart.CartID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cart);
        //}

        //// GET: Carts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts
        //        .FirstOrDefaultAsync(m => m.CartID == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return Problem("Entity set 'ASMNhom3Context.Carts'  is null.");
        //    }
        //    var cart = await _context.Carts.FindAsync(id);
        //    if (cart != null)
        //    {
        //        _context.Carts.Remove(cart);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CartExists(int id)
        //{
        //  return (_context.Carts?.Any(e => e.CartID == id)).GetValueOrDefault();
        //}
    }
}
