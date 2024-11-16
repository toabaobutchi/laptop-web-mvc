using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DipGiamGiaController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index()
        {
            return View(db.DipGiamGia);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DipGiamGia dgg)
        {
            if (ModelState.IsValid)
            {
                if (dgg.NgayBatDau.CompareTo(dgg.NgayHetHan) > 0)
                {
                    ModelState.AddModelError(nameof(DipGiamGia.NgayHetHan), $"Hãy chọn một thời điểm sau {dgg.NgayBatDau}");
                }
                else
                {
                    db.DipGiamGia.Add(dgg);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(dgg);
        }
        public IActionResult DeleteConfirm(int id)
        {
            return View(db.DipGiamGia.Find(id));
        }
        public IActionResult Delete(int id)
        {
            var dgg = db.DipGiamGia.Include(d => d.ThongTinGiamGia).Single(d => d.MaDipGiamGia == id);
            if (dgg != null)
            {
                db.DipGiamGia.Remove(dgg);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            return View(db.DipGiamGia.Find(id));
        }
        [HttpPost]
        public IActionResult Update(DipGiamGia d)
        {
            if (ModelState.IsValid)
            {
                if (d.NgayBatDau.CompareTo(d.NgayHetHan) > 0)
                {
                    ModelState.AddModelError(nameof(DipGiamGia.NgayHetHan), $"Hãy chọn một thời điểm sau {d.NgayBatDau}");
                }
                else
                {
                    var dgg = db.DipGiamGia.Find(d.MaDipGiamGia);
                    if (dgg != null)
                    {
                        dgg.TenDipGiamGia = d.TenDipGiamGia;
                        dgg.NgayBatDau = d.NgayBatDau;
                        dgg.NgayHetHan = d.NgayHetHan;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(d);
        }
    }
}
