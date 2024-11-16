using Microsoft.AspNetCore.Mvc;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomePageController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginAdmin(UserForm user)
        {
            var nguoiDung = db.NguoiDungs.FirstOrDefault(n => n.Email == user.Email && SHA256Hasher.Hash(user.Password) == n.MatKhau);
            if (nguoiDung == null)
            {
                ViewBag.ErrorMessage = "Email hoặc mật khẩu không chính xác";
                return View(user);
            }
            else
            {
                if (nguoiDung.IsAdmin)
                {
                    HttpContext.Session.Set("admin", nguoiDung);
                    return RedirectToAction("Index", "SanPham", new { area = "Admin" });
                }
                else
                {
                    ViewBag.ErrorMessage = "Bạn không phải quản trị viên!";
                    return View(user);
                }
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("LoginAdmin");
        }
    }
}
