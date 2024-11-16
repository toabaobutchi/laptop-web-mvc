using MyLaptopWebsite.Models;

namespace MyLaptopWebsite.Utils
{
    public static class CartHelper
    {
        public static IEnumerable<ChiTietHoaDon> ToChiTietHoaDons(List<CartDetail> cart)
        {
            var result = cart.Select(c => new ChiTietHoaDon()
            {
                MaSp = c.ProductId,
                GiaBan = c.Price,
                SoLuong = c.Quantity
            });
            return result;
        }
        public static string getAddress(NguoiDung user)
        {
            string[] e = { user.Duong, user.Phuong, user.Quan, user.ThanhPho };
            string address = user.SoNha;

            foreach (var item in e)
            {
                if (!string.IsNullOrEmpty(item)) address += ", ";
                address += item;
            }
            return address;
        }
    }
}
