using System;
using System.Linq;

namespace MyWebApp.Models;

/// <summary>Quy tắc 5 tiêu chí: 2 + 3 + 2 + 2 + 1 = 10 điểm.</summary>
public static class QuyTacDiemRenLuyen
{
    public static readonly string[] TenTieuChi =
    [
        "Phẩm chất đạo đức",
        "Ý thức chấp hành",
        "Học tập, nghiên cứu",
        "Hoạt động chung",
        "Thực hiện nhiệm vụ"
    ];

    public static readonly double[] DiemToiDa = [2, 3, 2, 2, 1];

    public static double?[] LayDiem(HocVien h) =>
    [h.DiemTieuChi1, h.DiemTieuChi2, h.DiemTieuChi3, h.DiemTieuChi4, h.DiemTieuChi5];

    /// <summary>
    /// Cho phép lưu điểm chưa hoàn thiện; chỉ trả tổng khi đủ 5 điểm hợp lệ.
    /// Không thay đổi các điểm đang lưu tại đây.
    /// </summary>
    public static (bool HopLe, string ThongBao, double? TongDiem) KiemTra(HocVien h)
    {
        var diem = LayDiem(h);
        for (int i = 0; i < diem.Length; i++)
        {
            if (!diem[i].HasValue) continue;
            double giaTri = diem[i]!.Value;
            if (double.IsNaN(giaTri) || double.IsInfinity(giaTri) ||
                giaTri < 0 || giaTri > DiemToiDa[i])
                return (false,
                    $"Tiêu chí {i + 1} – {TenTieuChi[i]}: chỉ được nhập từ 0 đến {DiemToiDa[i]} điểm.",
                    null);
        }
        if (diem.Any(d => !d.HasValue)) return (true, "", null);
        return (true, "", Math.Round(diem.Sum(d => d!.Value), 2, MidpointRounding.AwayFromZero));
    }

    public static double? TongKhiDu(HocVien h)
    {
        var ketQua = KiemTra(h);
        return ketQua.HopLe ? ketQua.TongDiem : null;
    }

    public static bool DaChamDu(HocVien h) => TongKhiDu(h).HasValue;

    public static string XepLoai(double? tong) => tong switch
    {
        null => "Chưa chấm",
        >= 9.0 => "Xuất sắc",
        >= 8.0 => "Giỏi",
        >= 7.0 => "Khá",
        >= 5.0 => "Trung bình",
        _ => "Không xếp loại"
    };
}
