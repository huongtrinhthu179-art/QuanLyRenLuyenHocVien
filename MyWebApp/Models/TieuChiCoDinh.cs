using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;

namespace MyWebApp.Models;

public static class TieuChiCoDinh
{
    public sealed record Muc(int ThuTu, string Ten, string MoTa, double DiemToiDa);

    public static readonly Muc[] DanhSach =
    {
        new(1, "Quan điểm, lập trường, phẩm chất chính trị; đạo đức, lối sống; quan hệ cộng đồng",
               "Quan điểm, lập trường, phẩm chất chính trị, đạo đức, lối sống và quan hệ cộng đồng.", 2),
        new(2, "Chấp hành Điều lệnh CAND, các quy định, nội quy, quy chế",
               "Ý thức, thái độ và kết quả chấp hành Điều lệnh Công an nhân dân và quy định liên quan đến học viên.", 3),
        new(3, "Học tập, nghiên cứu khoa học",
               "Ý thức, thái độ và kết quả học tập, nghiên cứu khoa học.", 2),
        new(4, "Tham gia hoạt động tập thể, phong trào",
               "Ý thức, thái độ và kết quả tham gia các hoạt động tập thể, phong trào trong và ngoài Học viện.", 2),
        new(5, "Thực hiện nhiệm vụ được giao",
               "Ý thức, thái độ và kết quả thực hiện nhiệm vụ được giao.", 1),
    };

    public static async Task DamBaoAsync(AppDbContext db)
    {
        var tatCa = await db.TieuChiDanhGias.ToListAsync();
        var coThayDoi = false;

        foreach (var m in DanhSach)
        {
            var tc = tatCa.FirstOrDefault(x => x.TenTieuChi == m.Ten);
            if (tc == null)
            {
                tc = new TieuChiDanhGia
                {
                    TenTieuChi = m.Ten,
                    MoTa = m.MoTa,
                    DiemToiDa = m.DiemToiDa,
                    ThuTu = m.ThuTu,
                    DangSuDung = true
                };
                db.TieuChiDanhGias.Add(tc);
                tatCa.Add(tc);
                coThayDoi = true;
            }
            else if (tc.ThuTu != m.ThuTu || tc.DiemToiDa != m.DiemToiDa || !tc.DangSuDung || tc.MoTa != m.MoTa)
            {
                tc.ThuTu = m.ThuTu;
                tc.DiemToiDa = m.DiemToiDa;
                tc.MoTa = m.MoTa;
                tc.DangSuDung = true;
                coThayDoi = true;
            }
        }

        var tenCoDinh = DanhSach.Select(x => x.Ten).ToHashSet();
        foreach (var tc in tatCa.Where(x => x.DangSuDung && !tenCoDinh.Contains(x.TenTieuChi)))
        {
            tc.DangSuDung = false;
            coThayDoi = true;
        }

        if (coThayDoi) await db.SaveChangesAsync();
    }
}