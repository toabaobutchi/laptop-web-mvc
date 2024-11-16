using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLaptopWebsite.Models;

public partial class ThongSoKyThuat
{
    [Display(Name = "Mã thông số")]
    public int MaThongSo { get; set; }

    [Display(Name = "Card đồ hoạ")]
    public string? CardDoHoa { get; set; }
    [Display(Name = "CPU")]
    public string Cpu { get; set; } = null!;
    [Display(Name = "RAM")]
    public byte Ram { get; set; }
    [Display(Name = "Loại RAM")]
    public string LoaiRam { get; set; } = null!;
    [Display(Name = "Ổ cứng")]
    public string Ocung { get; set; } = null!;
    [Display(Name = "Màn hình cảm ứng")]
    public bool? ManHinhCamUng { get; set; }
    [Display(Name = "Tần số quét")]
    public short TanSoQuet { get; set; }
    [Display(Name = "Kích thước màn hình")]
    public float KichThuocManHinh { get; set; }
    [Display(Name = "Độ phân giải")]
    public string DoPhanGiai { get; set; } = null!;
    [Display(Name = "Công nghệ màn hình")]
    public string CongNgheManHinh { get; set; } = null!;
    [Display(Name = "Cổng kết nối")]
    public string CongKetNoi { get; set; } = null!;

    public string WebCam { get; set; } = null!;

    public string Bluetooth { get; set; } = null!;
    [Display(Name = "Kích thước")]
    public string KichThuocXungQuanh { get; set; } = null!;
    [Display(Name = "Trọng lượng")]
    public float TrongLuong { get; set; }
    [Display(Name = "Âm thanh")]
    public string AmThanh { get; set; } = null!;

    public string Pin { get; set; } = null!;
    [Display(Name = "Hệ điều hành")]
    public string HeDieuHanh { get; set; } = null!;
    [Display(Name = "Mã sản phẩm")]
    public int MaSp { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
