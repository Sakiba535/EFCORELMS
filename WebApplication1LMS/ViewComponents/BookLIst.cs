using Microsoft.AspNetCore.Mvc;
using WebApplication1LMS.Models;

namespace WebApplication1LMS.ViewComponents
{
    public class BookLIst:ViewComponent
    {
        public IViewComponentResult Invoke(List<Book> data)
        {

            ViewBag.Count = data.Count;
            ViewBag.Total = data.Sum(i => i.RentPrice);

            return View(data);
        }
    }
}
