using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using System.Diagnostics;

namespace MyLaptopWebsite.Controllers
{
    public class HomeController : Controller
    {
        LaptopDbContext db = new LaptopDbContext();
        public IActionResult Index()
        {
            ViewBag.Brands = db.NhaSanXuats.AsNoTracking();
            var promotionGroups = from s in db.SanPhams.Include(s => s.HinhAnhSanPhams).Include(s => s.ThongTinGiamGia).ThenInclude(t => t.MaDipGiamGiaNavigation)
                                  group s by s.ThongTinGiamGia.MaDipGiamGiaNavigation;
            return View(promotionGroups);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}