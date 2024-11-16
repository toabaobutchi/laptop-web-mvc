using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MyLaptopWebsite.Models;

namespace MyLaptopWebsite.Utils
{
    public class Filter
    {
        public const double ALL = 0;
        public const int PROMOTION = 0;
        public const int ASC = 1;
        public const int DESC = 2;

        public static IEnumerable<SanPham> filter(IEnumerable<SanPham> src, string q, int? brand, double? screen, int? sortBy, string cpu)
        {
            if (!string.IsNullOrEmpty(q)) // có tìm kiếm theo từ khoá
            {
                q = q.ToLower();
                src = src.Where(s => s.TenSp.ToLower().Contains(q));
            }

            // tìm kiếm theo nhà sản xuất - bran
            if (brand.HasValue && brand.Value != ALL)
            {
                src = src.Where(s => s.MaNsx == brand.Value);
            }

            // lọc theo kích thước màn hình
            if (screen.HasValue && screen.Value != ALL)
            {
                src = src.Where(s => s.ThongSoKyThuat?.KichThuocManHinh <= screen.Value);
            }

            if (sortBy.HasValue && src.Count() != 0)
            {
                switch (sortBy.Value)
                {
                    case ASC:
                        src = src.OrderBy(s => s.DonGia);
                        break;
                    case DESC:
                        src = src.OrderByDescending(s => s.DonGia);
                        break;
                    case PROMOTION:
                        src = src.Where(s => s.ThongTinGiamGia != null);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(cpu) && cpu != "0")
            {
                src = src.Where(s => s.ThongSoKyThuat?.Cpu.Contains(cpu) ?? false);
            }
            return src;
        }

    }
}
