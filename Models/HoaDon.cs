using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class HoaDon
{
    [Display(Name = "Mã đơn hàng")]
    public int MaDonHang { get; set; }
    [Display(Name = "Ngày lập")]
    public DateTime NgayLap { get; set; }
    [Display(Name = "Tình trạng đơn hàng")]
    public int? TinhTrang { get; set; }
    [Display(Name = "Ngày giao hàng")]
    public DateTime? NgayGiaoHang { get; set; }
    [Display(Name = "Thành tiền")]
    public decimal ThanhTien { get; set; }
    [Display(Name = "Địa chỉ giao hàng")]
    public string? DiaChi { get; set; }
    [Display(Name = "Mã người dùng")]
    public int MaNguoiDung { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
