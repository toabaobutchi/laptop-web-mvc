using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class NhaSanXuat
{
    [Display(Name = "Mã nhà sản xuất")]
    public int MaNsx { get; set; }
    [Display(Name = "Tên nhà sản xuất")]
    public string TenNsx { get; set; } = null!;
    [Display(Name = "Hình ảnh")]
    public string? Logo { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
