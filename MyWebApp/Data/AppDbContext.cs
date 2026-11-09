using Microsoft.AspNetCore.Identity;
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
    public DbSet<TieuChiDanhGia> TieuChiDanhGias => Set<TieuChiDanhGia>();
    public DbSet<KetQuaTieuChi> KetQuaTieuChis => Set<KetQuaTieuChi>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(profile => profile.UserId);
            entity.Property(profile => profile.UserId).HasMaxLength(450);
            entity.Property(profile => profile.FullName).HasMaxLength(150);
            entity.Property(profile => profile.Gender).HasMaxLength(30);
            entity.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(profile => profile.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
