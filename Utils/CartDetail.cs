using MyLaptopWebsite.Models;

namespace MyLaptopWebsite.Utils
{
    public class CartDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }

        public ChiTietHoaDon ToChiTietHoaDon()
        {
            return new ChiTietHoaDon()
            {
                SoLuong = Quantity,
                MaSp = ProductId,
                GiaBan = Price
            };
        }
    }
}
