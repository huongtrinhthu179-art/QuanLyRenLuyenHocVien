namespace MyWebApp.Models;

public class HocVien
{
    public int Id { get; set; }

    public string HoTen { get; set; } = "";

    public DateTime NgaySinh { get; set; } = DateTime.Today;

    public string Lop { get; set; } = "";

    public string DiaChiHienTai { get; set; } = "";

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

