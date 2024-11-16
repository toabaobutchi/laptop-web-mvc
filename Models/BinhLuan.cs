using System;
using System.Collections.Generic;

namespace MyLaptopWebsite.Models;

public partial class BinhLuan
{
    public string NoiDung { get; set; } = null!;

    public int MaBinhLuan { get; set; }

    public int? MaBinhLuanPhuThuoc { get; set; }

    public int MaSanPham { get; set; }

    public int MaNguoiDung { get; set; }

    public virtual ICollection<BinhLuan> InverseMaBinhLuanPhuThuocNavigation { get; set; } = new List<BinhLuan>();

    public virtual BinhLuan? MaBinhLuanPhuThuocNavigation { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
