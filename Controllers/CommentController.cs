using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Controllers
{
    public class CommentController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult AddComment(int pid, string comment)
        {
            try
            {
                if (string.IsNullOrEmpty(comment))
                {
                    var user = HttpContext.Session.Get<NguoiDung>("user");
                    BinhLuan bl = new BinhLuan()
                    {
                        MaSanPham = pid,
                        MaNguoiDung = user.MaNguoiDung,
                        NoiDung = comment,
                        MaBinhLuanPhuThuoc = null
                    };
                    db.BinhLuans.Add(bl);
                    db.SaveChanges();
                    return Json(new { status = true, uname = user.TenNguoiDung });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        public IActionResult AddReply(int pid, string reply, int commentId)
        {
            try
            {
                if (string.IsNullOrEmpty(reply))
                {
                    var user = HttpContext.Session.Get<NguoiDung>("user");
                    BinhLuan bl = new BinhLuan()
                    {
                        MaSanPham = pid,
                        MaNguoiDung = user.MaNguoiDung,
                        NoiDung = reply,
                        MaBinhLuanPhuThuoc = commentId
                    };
                    db.BinhLuans.Add(bl);
                    db.SaveChanges();
                    return Json(new { status = true, uname = user.TenNguoiDung });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            catch
            {
                return Json(new { status = false });
            }
        }
    }
}
