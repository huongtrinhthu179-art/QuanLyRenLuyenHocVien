using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<HocVien> HocViens => Set<HocVien>();

    public DbSet<KyDanhGia> KyDanhGias => Set<KyDanhGia>();

    public DbSet<TieuChiDanhGia> TieuChiDanhGias
        => Set<TieuChiDanhGia>();
    public DbSet<KetQuaTieuChi> KetQuaTieuChis => Set<KetQuaTieuChi>();
}