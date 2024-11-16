using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class NguoiDung
{
    [Display(Name = "Mã người dùng")]
    public int MaNguoiDung { get; set; }
    [Display(Name = "Tên người dùng")]
    public string? TenNguoiDung { get; set; }

    public string Email { get; set; } = null!;
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = null!;
    [Display(Name = "Số nhà")]
    public string? SoNha { get; set; }
    [Display(Name = "Đường")]
    public string? Duong { get; set; }
    [Display(Name = "Phường/Xã/Thị trấn")]
    public string? Phuong { get; set; }
    [Display(Name = "Quận/Huyện")]
    public string? Quan { get; set; }
    [Display(Name = "Thành phố/Tỉnh")]
    public string? ThanhPho { get; set; }

    [Display(Name = "Chức vụ")]
    public bool IsAdmin { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
