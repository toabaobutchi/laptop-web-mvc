using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index()
        {
            return View(db.HoaDons.OrderByDescending(h => h.NgayLap).Include(h => h.MaNguoiDungNavigation).AsNoTracking());
        }
        public IActionResult FilterOrder(int? orderType)
        {
            orderType = orderType ?? -1;
            ViewBag.OrderType = orderType;
            if (orderType == -1)
            {
                return PartialView(
                    "_pvOrderTable",
                    db.HoaDons
                        .OrderByDescending(h => h.NgayLap)
                        .Include(h => h.MaNguoiDungNavigation).AsNoTracking()
                );
            }
            return PartialView(
                "_pvOrderTable",
                db.HoaDons
                    .OrderByDescending(h => h.NgayLap)
                    .Where(h => h.TinhTrang == orderType)
                    .Include(h => h.MaNguoiDungNavigation).AsNoTracking()
            );
        }
        public async Task<IActionResult> ShipOrder(int oid)
        {
            var order = db.HoaDons.Find(oid);
            if (order != null)
            {
                order.TinhTrang = 1;
                order.NgayGiaoHang = DateTime.Now;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Finish(int oid)
        {
            var order = db.HoaDons.Find(oid);
            if (order != null)
            {
                order.TinhTrang = 2;
                order.NgayGiaoHang = DateTime.Now;
                db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
