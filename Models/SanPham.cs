using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class SanPham
{
    [Display(Name = "Mã sản phẩm")]
    public int MaSp { get; set; }
    [Display(Name = "Tên sản phẩm")]
    public string TenSp { get; set; } = null!;
    [Display(Name = "Đơn giá")]
    public decimal DonGia { get; set; }
    [Display(Name = "Mô tả")]
    public string BaiViet { get; set; } = null!;
    [Display(Name = "Tồn kho")]
    public int TonKho { get; set; }
    [Display(Name = "Mã nhà sản xuất")]
    public int MaNsx { get; set; }
    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();

    public virtual NhaSanXuat? MaNsxNavigation { get; set; }

    public virtual ThongSoKyThuat? ThongSoKyThuat { get; set; }

    public virtual ThongTinGiamGia? ThongTinGiamGia { get; set; }
}
