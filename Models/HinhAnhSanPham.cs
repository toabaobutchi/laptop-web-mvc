using System;
using System.Collections.Generic;

namespace MyLaptopWebsite.Models;

public partial class HinhAnhSanPham
{
    public int MaHinhAnh { get; set; }

    public string HinhAnh { get; set; } = null!;

    public int MaSp { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
