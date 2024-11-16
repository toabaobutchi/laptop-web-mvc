using System;
using System.Collections.Generic;

namespace MyLaptopWebsite.Models;

public partial class DanhGia
{
    public int MaSp { get; set; }

    public int MaNguoiDung { get; set; }

    public byte _DanhGia { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
