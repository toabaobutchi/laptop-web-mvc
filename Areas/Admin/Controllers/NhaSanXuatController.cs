using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Areas.Admin.Models;
using MyLaptopWebsite.Models;

namespace MyLaptopWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhaSanXuatController : Controller
    {
        LaptopDbContext db = new();
        public IActionResult Index() => View(db.NhaSanXuats.AsNoTracking());
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(NhaSanXuat nsx, IFormFile? logo)
        {
            if (ModelState.IsValid)
            {
                if (logo == null || logo.Length == 0)
                {
                    ModelState.AddModelError("", "Hãy tải lên ảnh làm logo cho nhà sản xuất");
                }
                else
                {
                    string etx = Path.GetExtension(logo.FileName);
                    if (!FileUtils.AcceptedImageExtensions.Contains(etx))
                    {
                        ModelState.AddModelError("", "Xin lỗi, định dạng ảnh không được hỗ trợ bởi hệ thống!");
                    }
                    else if (FileUtils.MAX_LENGTH_UPLOADED_FILE < logo.Length)
                    {
                        ModelState.AddModelError("", $"Dung lượng file {logo.Length} byte không được hỗ trợ. Hãy chọn ảnh có dung lượng dưới {FileUtils.MAX_LENGTH_UPLOADED_FILE} byte");
                    }
                    else
                    {
                        try
                        {
                            string path = FileUtils.UPLOADED_FILE_PATH;
                            //Directory.CreateDirectory(path); // tạo thư mục Uploads nếu chưa tồn tại
                            using (var stream = System.IO.File.Create(path + "/" + logo.FileName))
                            {
                                logo.CopyTo(stream);
                            }
                            nsx.Logo = logo.FileName;
                            db.NhaSanXuats.Add(nsx);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Lỗi nội bộ đã xảy ra khi thêm nhà sản xuất!");
                        }
                    }
                }
            }
            return View(nsx);
        }

        public IActionResult DeleteConfirm(int id)
        {
            var nsx = db.NhaSanXuats.Include(n => n.SanPhams).FirstOrDefault(n => n.MaNsx == id);
            return View(nsx);
        }
        public IActionResult Delete(int id)
        {
            var nsx = db.NhaSanXuats.Find(id);
            if (nsx != null)
            {
                var logo = nsx.Logo;
                db.NhaSanXuats.Remove(nsx);
                db.SaveChanges();
                System.IO.File.Delete(@$"wwwroot/images/{logo}"); // xoá logo của nhà sản xuất trong máy
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var nsx = db.NhaSanXuats.Find(id);
            return View(nsx);
        }

        [HttpPost]
        public IActionResult Update(NhaSanXuat nsx, IFormFile? newLogo)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                var n = db.NhaSanXuats.Find(nsx.MaNsx);
                if (n != null)
                {
                    n.TenNsx = nsx.TenNsx;
                    if (FileUtils.CheckUploadedFile(newLogo, out msg, allowNull: true))
                    {
                        if (newLogo != null && newLogo.Length != 0)
                        {
                            System.IO.File.Delete(FileUtils.UPLOADED_FILE_PATH + "/" + n.Logo); // xoá file logo cũ
                            FileUtils.SaveUploadedFile(FileUtils.UPLOADED_FILE_PATH, newLogo);
                            n.Logo = newLogo.FileName; // cập nhật lại
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ModelState.AddModelError("", msg);
            return View(nsx);
        }
    }
}
