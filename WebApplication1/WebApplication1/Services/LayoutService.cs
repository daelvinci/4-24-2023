using WebApplication1.DAL;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Packaging.Signing;

using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public class LayoutService
    {
        private readonly IHttpContextAccessor _accessor;

        private PustokDbContext _context { get; set; }
        public LayoutService(PustokDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }
        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }

        public BasketViewModel GetBasket()
        {
            BasketViewModel basketVM = new BasketViewModel();
            List<BasketCookieItemViewModel> basketItems = new List<BasketCookieItemViewModel>();
            var basket = _accessor.HttpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);

            }
            foreach (var item in basketItems)
            { 
                
                var book = _context.Books.Include(x=>x.BookImages.Where(bi=>bi.PosterStatus==true))?.FirstOrDefault(x => x.Id == item.BookId);
                basketVM.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count

                });
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                basketVM.TotalPrice += (price*item.Count);
                
                
            }
            return basketVM;
        }


    }
}
