namespace MyWebApp.Models;

public class HocVien
{
    public int Id { get; set; }

    public string HoTen { get; set; } = "";

    public DateTime NgaySinh { get; set; } = DateTime.Today;

    public string Lop { get; set; } = "";

    public string DiaChiHienTai { get; set; } = "";

    // Thông tin bổ sung theo góp ý của nhóm. Tùy chọn để không làm hỏng bản ghi cũ.
    public string QueQuan { get; set; } = "";
    public string SoBaoDanh { get; set; } = "";
    public string GioiTinh { get; set; } = "";
    public string Email { get; set; } = "";
    public string SoDienThoai { get; set; } = "";
    public string DanhGia { get; set; } = "";

    // Điểm 5 tiêu chí (0..10), để nullable nhằm hỗ trợ hồ sơ cũ chưa có điểm chi tiết.
    // Không tự cộng/trung bình khi chưa có quy chế hệ số và quy tắc tổng hợp của nhóm.
    public double? DiemTieuChi1 { get; set; }
    public double? DiemTieuChi2 { get; set; }
    public double? DiemTieuChi3 { get; set; }
    public double? DiemTieuChi4 { get; set; }
    public double? DiemTieuChi5 { get; set; }

    public double DiemRenLuyen { get; set; } = 8.0;

    public string XepLoai => DiemRenLuyen switch
    {
        >= 9.0 => "Xuất sắc",
        >= 8.0 => "Giỏi",
        >= 7.0 => "Khá",
        >= 5.0 => "Trung bình",
        _ => "Không xếp loại"
    };
}

