using System;
using System.Collections.Generic;

namespace MyLaptopWebsite.Models;

public partial class ChiTietHoaDon
{
    public int MaChiTietHoaDon { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaBan { get; set; }

    public int MaSp { get; set; }

    public int MaDonHang { get; set; }

    public virtual HoaDon MaDonHangNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
