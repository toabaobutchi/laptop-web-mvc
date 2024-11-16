using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;
using NuGet.Versioning;
using System.Security.Cryptography;
using X.PagedList;

namespace MyLaptopWebsite.Controllers
{
    public class ShopController : Controller
    {
        LaptopDbContext db = new LaptopDbContext();
        public IActionResult Index()
        {
            var sp = db.SanPhams
                .Include(s => s.HinhAnhSanPhams)
                .Include(s => s.ThongTinGiamGia)
                .ThenInclude(t => t.MaDipGiamGiaNavigation)
                .Where(s => s.TonKho > 0)
                .AsNoTracking().ToPagedList(1, 4);
            ViewBag.TSKT = db.ThongSoKyThuats.AsNoTracking();
            ViewBag.HangSX = db.NhaSanXuats.AsNoTracking();
            return View(sp);
        }
        public IActionResult ProductDetail(int pid)
        {
            var sp = db.SanPhams
                .Include(s => s.ThongSoKyThuat)
                .Include(s => s.HinhAnhSanPhams)
                .Include(s => s.DanhGia)
                .Include(s => s.ThongTinGiamGia).AsNoTracking()
                .Single(s => s.MaSp == pid);
            ViewBag.OtherProducts = db.SanPhams.Where(s => s.MaNsx == sp.MaNsx).Include(s => s.ThongTinGiamGia).Include(s => s.HinhAnhSanPhams).AsNoTracking().Take(6);

            return View(sp);
        }
        public IActionResult Search(string q, int? brand, double? screen, int? sortBy, string cpu, int? page)
        {
            ViewBag.SearchString = q;
            ViewBag.Brand = brand;
            ViewBag.Screen = screen;
            ViewBag.SortBy = sortBy;
            ViewBag.Cpu = cpu;

            // danh sách sản phẩm chưa được lọc
            IEnumerable<SanPham> searchResults = db.SanPhams
                    .Include(s => s.HinhAnhSanPhams)
                    .Include(s => s.ThongSoKyThuat)
                    .Include(s => s.ThongTinGiamGia)
                    .ThenInclude(t => t.MaDipGiamGiaNavigation)
                    .Where(s => s.TonKho > 0)
                    .AsNoTracking();

            searchResults = Filter.filter(searchResults, q, brand, screen, sortBy, cpu).ToPagedList(page ?? 1, 4);

            ViewBag.TSKT = db.ThongSoKyThuats.AsNoTracking();
            ViewBag.HangSX = db.NhaSanXuats.AsNoTracking();

            return View("Views/Shop/Index.cshtml", searchResults);
        }
        public IActionResult RenderContent(string contentType, int pid)
        {
            if (contentType == "relatedProducts")
            {
                var model = db.SanPhams.Where(s => s.MaNsx == pid)
                    .Include(s => s.ThongTinGiamGia)
                    .Include(s => s.HinhAnhSanPhams)
                    .AsNoTracking().Take(6);
                ViewBag.MaSp = pid;

                return PartialView("_pvRelatedProduct", model);
            }
            else
            {
                var sp = db.SanPhams
                    .Include(s => s.DanhGia)
                    .AsNoTracking().FirstOrDefault(s => s.MaSp == pid);

                // lấy các bình luận của sản phẩm
                var comments = db.BinhLuans
                    .Where(b => b.MaBinhLuanPhuThuoc == null)
                    .Include(b => b.MaNguoiDungNavigation)
                    .Include(b => b.InverseMaBinhLuanPhuThuocNavigation).ThenInclude(ib => ib.MaNguoiDungNavigation).Where(b => b.MaSanPham == pid);

                ViewBag.Comments = comments; // bỏ qua các bình luận chính, vì chúng đã là Key cho tập bình luận khác
                var user = HttpContext.Session.Get<NguoiDung>("user");
                if (user != null)
                {
                    ViewBag.CurrentUserVote = db.DanhGia.FirstOrDefault(d => d.MaSp == pid && d.MaNguoiDung == user.MaNguoiDung); // dùng cho _pvVoteAndComments.cshtml
                }

                return PartialView("_pvVoteAndComments", sp);
            }
        }
        public IActionResult Vote(int id, int[] voteStar)
        {
            var sp = db.SanPhams.Find(id);
            var user = HttpContext.Session.Get<NguoiDung>("user");
            if (sp != null && voteStar != null && voteStar.Length != 0) // xử lý trường hợp khách hàng không nhập đánh giá
            {
                var vote = db.DanhGia.FirstOrDefault(s => s.MaSp == id && s.MaNguoiDung == user.MaNguoiDung);
                if (vote != null)
                {
                    vote._DanhGia = (byte)voteStar[voteStar.Length - 1]; // cập nhật lại
                }
                else
                {
                    sp.DanhGia.Add(new DanhGia() { _DanhGia = (byte)voteStar[voteStar.Length - 1], MaNguoiDung = user.MaNguoiDung });
                }

                db.SaveChanges();
            }
            return RedirectToAction("ProductDetail", new { pid = id });
        }
    }
}
