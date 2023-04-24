using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Packaging.Signing;
using WebApplication1.DAL;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private PustokDbContext _context;
        public BookController(PustokDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var book = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.BookImages)
                .FirstOrDefault(x => x.Id == id);

            return PartialView("BookModalPartial", book);
        }

        public IActionResult AddToBasket(int id)
        {
            if(_context.Books.Find(id)==null)
            {
                return NotFound();
            }

            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketCookieItemViewModel> basketItems;

            if (basket == null)
                basketItems = new List<BasketCookieItemViewModel>();
            else
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);

            var wantedBook = basketItems.FirstOrDefault(x => x.BookId == id);
            if (wantedBook != null)
                wantedBook.Count++;
            else
                basketItems.Add(new BasketCookieItemViewModel { BookId = id, Count = 1 });

            BasketViewModel basketVM = new BasketViewModel();

            foreach (var item in basketItems)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(bi => bi.PosterStatus == true))?.FirstOrDefault(x => x.Id == item.BookId);
                basketVM.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count

                });
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                basketVM.TotalPrice += (price * item.Count);


            }

            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));
            //return RedirectToAction("Index", "Home");
            return PartialView("BookCartPartial", basketVM);
        }

        public IActionResult ShowBasket()
        {
            var basket = HttpContext.Request.Cookies["basket"];
            var basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);
            return Json(basketItems);
        }

        public IActionResult DeleteFromBasket(int id)
        {

            if (_context.Books.Find(id) == null)
            {
                return NotFound();
            }

            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketCookieItemViewModel> basketItems;

            if (basket == null)
                basketItems = new List<BasketCookieItemViewModel>();
            else
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);


            var toDelete = basketItems.FirstOrDefault(x => x.BookId == id);
            basketItems.Remove(toDelete);

            BasketViewModel basketVM = new BasketViewModel();

            foreach (var item in basketItems)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(bi => bi.PosterStatus == true))?.FirstOrDefault(x => x.Id == item.BookId);
                basketVM.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count

                });
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                basketVM.TotalPrice += (price * item.Count);
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));

            return PartialView("BookCartPartial", basketVM);
        }
    }
}
