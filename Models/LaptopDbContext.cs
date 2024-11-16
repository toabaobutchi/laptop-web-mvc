using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyLaptopWebsite.Models;

public partial class LaptopDbContext : DbContext
{
    public LaptopDbContext()
    {
    }

    public LaptopDbContext(DbContextOptions<LaptopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<DanhGia> DanhGia { get; set; }

    public virtual DbSet<DipGiamGia> DipGiamGia { get; set; }

    public virtual DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<ThongSoKyThuat> ThongSoKyThuats { get; set; }

    public virtual DbSet<ThongTinGiamGia> ThongTinGiamGia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Initial Catalog=LaptopDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A04364DD3D");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.NoiDung).HasMaxLength(1000);

            entity.HasOne(d => d.MaBinhLuanPhuThuocNavigation).WithMany(p => p.InverseMaBinhLuanPhuThuocNavigation)
                .HasForeignKey(d => d.MaBinhLuanPhuThuoc)
                .HasConstraintName("FK__BinhLuan__MaBinh__5BE2A6F2");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BinhLuan__MaNguo__5DCAEF64");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BinhLuan__MaSanP__5CD6CB2B");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaChiTietHoaDon).HasName("PK__ChiTietH__CFF2C4260F8BDFBC");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(10, 0)");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.ToTable(t => t.HasTrigger("trg_update_quantity")); // khai báo trigger

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaDon__5629CD9C");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHoa__MaSP__5535A963");
        });

        modelBuilder.Entity<DanhGia>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaSp }).HasName("PK__DanhGia__174B87E3C149181A");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e._DanhGia).HasColumnName("DanhGia");
            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaNguoi__4BAC3F29");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaSP__4CA06362");
        });

        modelBuilder.Entity<DipGiamGia>(entity =>
        {
            entity.HasKey(e => e.MaDipGiamGia).HasName("PK__DipGiamG__4B3F662C42E0522B");

            entity.ToTable(tb => tb.HasTrigger("CheckNgayHetHan"));

            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.TenDipGiamGia).HasMaxLength(50);
        });

        modelBuilder.Entity<HinhAnhSanPham>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HinhAnhS__A9C37A9B27E8ACEE");

            entity.ToTable("HinhAnhSanPham");

            entity.Property(e => e.HinhAnh)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.HinhAnhSanPhams)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HinhAnhSan__MaSP__59063A47");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__HoaDon__129584AD494A6AF7");

            entity.ToTable("HoaDon");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.NgayGiaoHang).HasColumnType("datetime");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TinhTrang).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HoaDon__MaNguoiD__5165187F");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D7627BF7C605");

            entity.ToTable("NguoiDung");
            entity.Property(e => e.Duong).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phuong).HasMaxLength(50);
            entity.Property(e => e.Quan).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SoNha).HasMaxLength(50);
            entity.Property(e => e.TenNguoiDung).HasMaxLength(100);
            entity.Property(e => e.ThanhPho).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaSanXuat>(entity =>
        {
            entity.HasKey(e => e.MaNsx).HasName("PK__NhaSanXu__3A1BDBD2411FD810");

            entity.ToTable("NhaSanXuat");

            entity.Property(e => e.MaNsx).HasColumnName("MaNSX");
            entity.Property(e => e.Logo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenNsx)
                .HasMaxLength(50)
                .HasColumnName("TenNSX");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081CC98267F5");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.BaiViet).HasMaxLength(1000);
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MaNsx).HasColumnName("MaNSX");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaNsxNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaNsx)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SanPham__MaNSX__3D5E1FD2");
        });

        modelBuilder.Entity<ThongSoKyThuat>(entity =>
        {
            entity.HasKey(e => e.MaThongSo).HasName("PK__ThongSoK__60E16E647DE24C92");

            entity.ToTable("ThongSoKyThuat");

            entity.HasIndex(e => e.MaSp, "UQ__ThongSoK__2725081D6FC1624D").IsUnique();

            entity.Property(e => e.AmThanh).HasMaxLength(500);
            entity.Property(e => e.Bluetooth).HasMaxLength(500);
            entity.Property(e => e.CardDoHoa).HasMaxLength(500);
            entity.Property(e => e.CongKetNoi).HasMaxLength(500);
            entity.Property(e => e.CongNgheManHinh).HasMaxLength(100);
            entity.Property(e => e.Cpu)
                .HasMaxLength(500)
                .HasColumnName("CPU");
            entity.Property(e => e.DoPhanGiai).HasMaxLength(50);
            entity.Property(e => e.HeDieuHanh).HasMaxLength(20);
            entity.Property(e => e.KichThuocXungQuanh).HasMaxLength(500);
            entity.Property(e => e.LoaiRam)
                .HasMaxLength(500)
                .HasColumnName("LoaiRAM");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.Ocung)
                .HasMaxLength(500)
                .HasColumnName("OCung");
            entity.Property(e => e.Pin).HasMaxLength(500);
            entity.Property(e => e.Ram).HasColumnName("RAM");
            entity.Property(e => e.WebCam).HasMaxLength(500);

            entity.HasOne(d => d.MaSpNavigation).WithOne(p => p.ThongSoKyThuat)
                .HasForeignKey<ThongSoKyThuat>(d => d.MaSp)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ThongSoKyT__MaSP__46E78A0C");
        });

        modelBuilder.Entity<ThongTinGiamGia>(entity =>
        {
            entity.HasKey(e => e.MaGiamGia).HasName("PK__ThongTin__EF9458E4B4087150");

            entity.HasIndex(e => e.MaSp, "UQ__ThongTin__2725081D51E97FA3").IsUnique();

            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaDipGiamGiaNavigation).WithMany(p => p.ThongTinGiamGia)
                .HasForeignKey(d => d.MaDipGiamGia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ThongTinGi__MaSP__4222D4EF");

            entity.HasOne(d => d.MaSpNavigation).WithOne(p => p.ThongTinGiamGia)
                .HasForeignKey<ThongTinGiamGia>(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThongTinGi__MaSP__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
