using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Controllers
{
    public class UserController : Controller
    {
        LaptopDbContext db = new LaptopDbContext();
        public IActionResult Profile()
        {
            ViewBag.Message = TempData["message"];
            ViewBag.Status = TempData["status"];
            return View();
        }

        public IActionResult Update(NguoiDung n)
        {
            var user = HttpContext.Session.Get<NguoiDung>("user");
            string message = "Cập nhật thông tin thất bại";
            bool status = false;
            if (user != null) // có session
            {
                user.TenNguoiDung = n.TenNguoiDung;
                user.ThanhPho = n.ThanhPho;
                user.Quan = n.Quan;
                user.Duong = n.Duong;
                user.Phuong = n.Phuong;
                user.SoDienThoai = n.SoDienThoai;
                user.SoNha = n.SoNha;
                db.Update(user);
                db.SaveChanges();
                HttpContext.Session.Set("user", user);
                message = "Đã cập nhật thành công";
                status = true;
            }
            TempData["message"] = message;
            TempData["status"] = status;
            return RedirectToAction("Profile");
        }

        public IActionResult GetUserContent(string contentType)
        {
            var user = HttpContext.Session.Get<NguoiDung>("user");
            if (user != null)
            {
                switch (contentType)
                {
                    case "orders":
                        var orders = db.HoaDons.Where(h => h.MaNguoiDung == user.MaNguoiDung);
                        return PartialView("_pvOrders", orders);
                    case "info":
                        return PartialView("_pvProfile");
                    default:
                        return Content("Nội dung yêu cầu không hợp lệ");
                }

            }
            return Content("Không thể tải dữ liệu");
        }

        public IActionResult GetOrderDetails(int oid)
        {
            var order = db.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(c => c.MaSpNavigation)
                .ThenInclude(s => s.HinhAnhSanPhams)
                .FirstOrDefault(h => h.MaDonHang == oid);
            return PartialView("_pvOrderDetails", order);
        }

        public IActionResult RenderForm(string form)
        {
            if (form == "signup")
            {
                return PartialView("_pvSignUpForm");
            }
            else if (form == "login")
            {
                return PartialView("_pvLoginForm");
            }
            return Content("Tài nguyên yêu cầu không tồn tại");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        public IActionResult Submit(IFormCollection form)
        {
            if (form.ContainsKey("logInBtn")) // form đăng nhập được submit
            {
                UserForm userForm = new UserForm()
                {
                    Email = form["Email"].ToString(),
                    Password = form["Password"].ToString()
                };

                var n = db.NguoiDungs.FirstOrDefault(n => n.Email == userForm.Email && n.MatKhau == SHA256Hasher.Hash(userForm.Password));

                if (n != null) // đăng nhập thành công
                {
                    HttpContext.Session.Set("user", n);
                    return Json(new { status = true, uname = n.TenNguoiDung });
                }
                else
                {
                    return Json(new { status = false, error = "Email hoặc mật khẩu không đúng" });
                }
            }
            else // form đăng ký
            {
                UserForm userForm = new UserForm()
                {
                    Email = form["Email"].ToString(),
                    Password = form["Password"].ToString(),
                    Username = form["Username"].ToString(),
                };
                if (db.NguoiDungs.Any(n => n.Email == userForm.Email)) // email đã được sử dụng
                {
                    return Json(new { status = false, error = "Email đã được sử dụng. Vui lòng chọn một địa chỉ khác" });
                }
                else // ok
                {
                    NguoiDung n = new NguoiDung()
                    {
                        Email = userForm.Email,
                        TenNguoiDung = userForm.Username,
                        MatKhau = SHA256Hasher.Hash(userForm.Password)
                    };
                    db.NguoiDungs.Add(n);
                    db.SaveChanges();
                    HttpContext.Session.Set("user", n);
                    return Json(new { status = true, uname = n.TenNguoiDung });
                }
            }
        }
    }
}
