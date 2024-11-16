using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NguoiDungController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index()
        {
            return View(db.NguoiDungs.AsNoTracking());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(NguoiDung n)
        {
            n.IsAdmin = true;
            n.MatKhau = SHA256Hasher.Hash(n.MatKhau);
            db.NguoiDungs.Add(n);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GrantPermission(int uid)
        {
            var user = db.NguoiDungs.Find(uid);
            if (user != null)
            {
                user.IsAdmin = true;
                await db.SaveChangesAsync();
                return Content("<p class='text-danger fw-bold'>Quản trị viên</p>");
            }
            return Content("");
        }
    }
}
