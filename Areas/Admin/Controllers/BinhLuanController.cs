using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BinhLuanController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index()
        {
            return View(db.BinhLuans
                .Where(b => b.MaBinhLuanPhuThuoc == null)
                .Include(b => b.MaSanPhamNavigation)
                .Include(b => b.MaNguoiDungNavigation)
                .Include(b => b.InverseMaBinhLuanPhuThuocNavigation).ThenInclude(r => r.MaNguoiDungNavigation)
            ); // tìm các bình luận chính - không phải trả lời
        }
        public IActionResult RenderContent(int commentId, string contentType)
        {
            if (contentType == "replies")
            {
                ViewBag.Comment = db.BinhLuans.Find(commentId);
                var replies = db.BinhLuans.Where(b => b.MaBinhLuanPhuThuoc == commentId).Include(b => b.MaNguoiDungNavigation);
                return PartialView("~/Areas/Admin/Views/Shared/_pvReplies.cshtml", replies);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddReply(int commentId, string reply)
        {
            var admin = HttpContext.Session.Get<NguoiDung>("admin");
            var bl = db.BinhLuans.Find(commentId);
            if (bl != null)
            {
                bl.InverseMaBinhLuanPhuThuocNavigation.Add(new BinhLuan()
                {
                    MaNguoiDung = admin.MaNguoiDung,
                    NoiDung = reply,
                    MaSanPham = bl.MaSanPham
                });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
