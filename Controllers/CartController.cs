using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLaptopWebsite.Models;
using MyLaptopWebsite.Utils;

namespace MyLaptopWebsite.Controllers
{
    public class CartController : Controller
    {
        LaptopDbContext db = new LaptopDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddToCart(int pid, int count)
        {
            int addedCount = 0;
            int quantity = count;
            try
            {
                var sp = db.SanPhams
                    .Include(s => s.ThongTinGiamGia)
                    .Include(s => s.HinhAnhSanPhams)
                    .AsNoTracking()
                    .SingleOrDefault(s => s.MaSp == pid);

                if (sp != null)
                {
                    if (count > sp.TonKho) // trường hợp không thể thêm do hết hàng
                    {
                        return Json(new { status = false, errorMessage = $"Không thể thêm {count} sản phẩm vào giỏ hàng" });
                    }

                    decimal giamgia = sp.ThongTinGiamGia != null ? 1 - (sp.ThongTinGiamGia.MucGiamGia / 100.0m) : 1;
                    CartDetail ct = new CartDetail()
                    {
                        ProductId = sp.MaSp,
                        Price = sp.DonGia * giamgia,
                        ProductName = sp.TenSp,
                        Quantity = count,
                        Image = sp.HinhAnhSanPhams.First().HinhAnh,
                        Stock = sp.TonKho
                    };

                    List<CartDetail> cart = HttpContext.Session.Get<List<CartDetail>>("cart");
                    if (cart == null) // chưa khởi tạo giỏ hàng
                    {
                        cart = new List<CartDetail> { ct };
                        addedCount++;
                    }
                    else // đã có giỏ hàng
                    {
                        var productInCart = cart.SingleOrDefault(c => c.ProductId == pid);

                        if (productInCart == null) // chưa có sản phẩm trong giỏ hàng
                        {
                            cart.Add(ct);
                            addedCount++;
                        }
                        else // đã có sản phẩm trong giỏ hàng
                        {
                            if (productInCart.Quantity + count <= productInCart.Stock)
                            {
                                productInCart.Quantity += count;
                            }
                            else
                            {
                                return Json(new { status = false, errorMessage = $"Không còn sản phẩm để thêm vào giỏ hàng" });
                            }
                        }
                    }
                    HttpContext.Session.Set("cart", cart);
                }
                return Json(new { status = true, count = addedCount, quantity = quantity });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, errorMessage = "Không thể thêm vào giỏ hàng" });
            }
        }
        public IActionResult ChangeQuantity(int pid, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartDetail>>("cart");
            var sp = cart.FirstOrDefault(c => c.ProductId == pid);
            if (quantity < sp.Stock)
            {
                sp.Quantity = quantity;
                HttpContext.Session.Set("cart", cart);
            }
            return Json(new { quantity = sp.Quantity, totalPrice = cart.Sum(c => c.Quantity * c.Price) });
        }
        public IActionResult DeleteProductFromCart(int pid)
        {
            List<CartDetail> cart = HttpContext.Session.Get<List<CartDetail>>("cart");
            var sp = cart.FirstOrDefault(c => c.ProductId == pid);
            bool isRemoved = false;
            if (sp != null)
            {
                isRemoved = cart.Remove(sp);
                HttpContext.Session.Set("cart", cart);
            }
            return Json(new { isRemoved, totalPrice = cart.Sum(c => c.Quantity * c.Price) });
        }

        [HttpPost]
        public async Task<IActionResult> Order(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                ViewBag.Message = "Đơn hàng đã bị từ chối vì không có địa chỉ nhận hàng";
                return View("Index");
            }
            List<CartDetail> cart = HttpContext.Session.Get<List<CartDetail>>("cart");

            // xét đã đăng nhập hay chưa
            NguoiDung user = HttpContext.Session.Get<NguoiDung>("user");

            if (user == null)
            {
                ViewBag.Message = "Hãy đăng nhập để thực hiện thanh toán";
                return View("Index");
            }

            if (cart != null && cart.Count > 0)
            {
                var orderDetails = cart.Select(c => c.ToChiTietHoaDon());
                HoaDon order = new HoaDon()
                {
                    ChiTietHoaDons = orderDetails.ToList(),
                    MaNguoiDung = user.MaNguoiDung,
                    NgayLap = DateTime.Now,
                    ThanhTien = orderDetails.Sum(o => o.GiaBan * o.SoLuong),
                    DiaChi = address
                };
                db.HoaDons.Add(order);
                await db.SaveChangesAsync();

                HttpContext.Session.Remove("cart");
                return RedirectToAction("Index", "Shop");
            }
            ViewBag.Message = "Xảy ra lỗi khi lập đơn hàng";
            return View("Index");
        }
    }
}
