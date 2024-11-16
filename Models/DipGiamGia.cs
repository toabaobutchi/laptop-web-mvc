using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class DipGiamGia
{
    [Display(Name = "Mã dịp giảm giá")]
    public int MaDipGiamGia { get; set; }
    [Display(Name = "Ngày hết hạn")]
    public DateTime NgayHetHan { get; set; }
    [Display(Name = "Ngày bắt đầu")]
    public DateTime NgayBatDau { get; set; }
    [Display(Name = "Tên dịp giảm giá")]
    public string TenDipGiamGia { get; set; } = null!;

    public virtual ICollection<ThongTinGiamGia> ThongTinGiamGia { get; set; } = new List<ThongTinGiamGia>();
}
