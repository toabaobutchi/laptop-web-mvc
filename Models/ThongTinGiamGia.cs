using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class ThongTinGiamGia
{
    [Display(Name = "Mã giảm giá")]
    public int MaGiamGia { get; set; }
    [Display(Name = "Mức giảm giá")]
    public int MucGiamGia { get; set; }
    [Display(Name = "Mã dịp giảm giá")]
    public int MaDipGiamGia { get; set; }
    [Display(Name = "Mã sản phẩm")]
    public int MaSp { get; set; }

    public virtual DipGiamGia MaDipGiamGiaNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
