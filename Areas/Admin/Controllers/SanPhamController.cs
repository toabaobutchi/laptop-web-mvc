using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using System.Net.WebSockets;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index()
        {
            ViewBag.Nhasanxuat = new SelectList(db.NhaSanXuats.AsNoTracking(), nameof(NhaSanXuat.MaNsx), nameof(NhaSanXuat.TenNsx));
            return View(db.SanPhams.Include(s => s.ThongTinGiamGia).Include(s => s.MaNsxNavigation).AsNoTracking().OrderBy(s => s.DonGia));
        }
        public IActionResult Create()
        {
            // menu nhà sản xuất
            ViewBag.Mansx = new SelectList(db.NhaSanXuats.AsNoTracking(), nameof(NhaSanXuat.MaNsx), nameof(NhaSanXuat.TenNsx));

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SanPham sp, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (sp.DonGia >= 0 && sp.TonKho >= 0)
                {
                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            string path = @"wwwroot/images/";
                            string fname = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(image.FileName);
                            using (var stream = System.IO.File.Create(path + fname))
                            {
                                sp.HinhAnhSanPhams.Add(
                                    new HinhAnhSanPham()
                                    {
                                        HinhAnh = fname
                                    }
                                );
                                await image.CopyToAsync(stream);
                            }
                        }
                    }
                    db.SanPhams.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (sp.DonGia < 0)
                    {
                        ModelState.AddModelError(nameof(SanPham.DonGia), "Giá trị đơn giá không hợp lệ");
                    }
                    if (sp.DonGia < 0)
                    {
                        ModelState.AddModelError(nameof(SanPham.TonKho), "Giá trị số lượng tồn kho không hợp lệ");
                    }
                }
            }
            ViewBag.Mansx = new SelectList(db.NhaSanXuats.AsNoTracking(), nameof(NhaSanXuat.MaNsx), nameof(NhaSanXuat.TenNsx));
            return View(sp);
        }
        public IActionResult FilterProduct(int id)
        {
            ViewBag.Nhasanxuat = new SelectList(db.NhaSanXuats.AsNoTracking(), nameof(NhaSanXuat.MaNsx), nameof(NhaSanXuat.TenNsx), id);
            object model = null;
            if (id != 0)
            {
                model = db.SanPhams.Include(s => s.ThongTinGiamGia).Include(s => s.MaNsxNavigation).Where(s => s.MaNsx == id).AsNoTracking();
            }
            else
            {
                model = db.SanPhams.Include(s => s.ThongTinGiamGia).Include(s => s.MaNsxNavigation).AsNoTracking();
            }
            return PartialView("~/Areas/Admin/Views/Shared/_pvProductTable.cshtml", model);
        }
        public IActionResult GetProductImages(int id)
        {
            var images = db.HinhAnhSanPhams.Where(h => h.MaSp == id).AsNoTracking();
            var product = db.SanPhams.Find(id);
            ViewBag.Product = product;
            return PartialView("~/Areas/Admin/Views/Shared/_pvImagesDetails.cshtml", images);
        }
        [HttpPost]
        public async Task<IActionResult> AddImages(int productId, List<IFormFile> images)
        {
            var sp = db.SanPhams.Find(productId);
            if (sp != null)
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        string path = @"wwwroot/images/";
                        string fname = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(image.FileName);


                        using (var stream = System.IO.File.Create(path + fname))
                        {
                            sp.HinhAnhSanPhams.Add(
                                new HinhAnhSanPham()
                                {
                                    HinhAnh = fname
                                }
                            );
                            await image.CopyToAsync(stream);
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveProductImage(int iid)
        {
            var img = db.HinhAnhSanPhams.Find(iid);
            if (img != null)
            {
                System.IO.File.Delete(@"wwwroot/images/" + img.HinhAnh);
                db.HinhAnhSanPhams.Remove(img);
                await db.SaveChangesAsync();
            }
            return Ok();
        }

        public IActionResult Delete(int pid)
        {
            var sp = db.SanPhams.Include(s => s.HinhAnhSanPhams).AsNoTracking().Single(s => s.MaSp == pid);
            if (sp != null)
            {
                ViewBag.ChiTietHoaDon = db.ChiTietHoaDons.Where(c => c.MaSp == sp.MaSp).Count();
                return PartialView("~/Areas/Admin/Views/Shared/_pvDeletedProductDetails.cshtml", sp);
            }
            return Content("<p class='text-danger'>Không tìm thấy sản phẩm</p>");
        }
        [HttpPost]
        public IActionResult DeleteProduct(int pid)
        {
            var sp = db.SanPhams.Include(s => s.ThongTinGiamGia).Include(s => s.HinhAnhSanPhams).Include(s => s.ThongSoKyThuat).Single(s => s.MaSp == pid);
            if (sp != null)
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();
                foreach (var img in sp.HinhAnhSanPhams)
                {
                    System.IO.File.Delete($@"wwwroot/images/{img.HinhAnh}");
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int pid)
        {
            var sp = db.SanPhams.Find(pid);
            if (sp != null)
            {
                ViewBag.Nhasanxuat = new SelectList(db.NhaSanXuats.AsNoTracking(), nameof(NhaSanXuat.MaNsx), nameof(NhaSanXuat.TenNsx), sp.MaNsx);
            }
            return View(sp);
        }

        [HttpPost]
        public IActionResult Update(SanPham s)
        {
            var sp = db.SanPhams.Find(s.MaSp);
            if (s.DonGia > 0 && s.TonKho > 0)
            {

                if (sp != null && ModelState.IsValid)
                {
                    sp.TenSp = s.TenSp;
                    sp.BaiViet = s.BaiViet;
                    sp.DonGia = s.DonGia;
                    sp.TonKho = s.TonKho;
                    sp.MaNsx = s.MaNsx;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (sp.DonGia < 0)
                {
                    ModelState.AddModelError(nameof(SanPham.DonGia), "Giá trị đơn giá không hợp lệ");
                }
                if (sp.DonGia < 0)
                {
                    ModelState.AddModelError(nameof(SanPham.TonKho), "Giá trị số lượng tồn kho không hợp lệ");
                }
            }
            return View(sp);
        }
        public IActionResult Discount(int pid)
        {
            var sp = db.SanPhams.Include(s => s.ThongTinGiamGia).AsNoTracking().Single(s => s.MaSp == pid);
            ViewBag.Dipgiamgia = new SelectList(db.DipGiamGia.Where(d => d.NgayHetHan > DateTime.Now.AddDays(1)).AsNoTracking(), nameof(DipGiamGia.MaDipGiamGia), nameof(DipGiamGia.TenDipGiamGia), sp.ThongTinGiamGia?.MaDipGiamGia);
            return PartialView("~/Areas/Admin/Views/Shared/_pvDiscount.cshtml", sp);
        }
        [HttpPost]
        public IActionResult Discount(int pid, int magiamgia, int discount)
        {
            var sp = db.SanPhams.Include(p => p.ThongTinGiamGia).Single(s => s.MaSp == pid);
            if (discount > 0) // có giảm giá
            {
                if (sp.ThongTinGiamGia == null) // thêm
                {
                    sp.ThongTinGiamGia = new ThongTinGiamGia()
                    {
                        MaDipGiamGia = magiamgia,
                        MucGiamGia = discount,
                        MaSp = pid,
                    };
                }
                else // cập nhật
                {
                    sp.ThongTinGiamGia.MaDipGiamGia = magiamgia;
                    sp.ThongTinGiamGia.MucGiamGia = discount;
                }
            }
            else // trường hợp không tạo hoặc xoá thông tin giảm giá hiện tại
            {
                var ttgg = db.ThongTinGiamGia.FirstOrDefault(t => t.MaSp == pid);
                if (ttgg != null)
                {
                    db.Remove(ttgg);
                    db.SaveChanges();
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Spectification(int pid)
        {
            var tskt = db.ThongSoKyThuats.FirstOrDefault(t => t.MaSp == pid);
            ViewBag.ProductId = pid;
            return PartialView("~/Areas/Admin/Views/Shared/_pvSpectification.cshtml", tskt);
        }
        [HttpPost]
        public IActionResult SaveSpectification(ThongSoKyThuat t)
        {
            if (t.MaThongSo == 0) // thêm thông số kỹ thuật
            {
                db.ThongSoKyThuats.Add(t);
            }
            else // cập nhật thông số kỹ thuật
            {
                var tskt = db.ThongSoKyThuats.Find(t.MaThongSo);
                if (tskt != null)
                {
                    tskt.TanSoQuet = t.TanSoQuet;
                    tskt.Cpu = t.Cpu;
                    tskt.Bluetooth = t.Bluetooth;
                    tskt.AmThanh = t.AmThanh;
                    tskt.KichThuocManHinh = t.KichThuocManHinh;
                    tskt.CardDoHoa = t.CardDoHoa;
                    tskt.CongKetNoi = t.CongKetNoi;
                    tskt.CongNgheManHinh = t.CongNgheManHinh;
                    tskt.DoPhanGiai = t.DoPhanGiai;
                    tskt.HeDieuHanh = t.HeDieuHanh;
                    tskt.KichThuocXungQuanh = t.KichThuocXungQuanh;
                    tskt.LoaiRam = t.LoaiRam;
                    tskt.ManHinhCamUng = t.ManHinhCamUng;
                    tskt.Ocung = t.Ocung;
                    tskt.Pin = t.Pin;
                    tskt.Ram = t.Ram;
                    tskt.TrongLuong = t.TrongLuong;
                    tskt.WebCam = t.WebCam;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult RenderComment()
        {
            return View();
        }
    }
}
