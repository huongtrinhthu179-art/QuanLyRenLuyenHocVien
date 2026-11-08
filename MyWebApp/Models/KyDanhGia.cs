namespace MyWebApp.Models;

public class KyDanhGia
{
    public int Id { get; set; }

    public string TenKy { get; set; } = "";

    public DateTime NgayBatDau { get; set; } = DateTime.Today;

    public DateTime NgayKetThuc { get; set; } = DateTime.Today;

    // false: Đang mở; true: Đã khóa
    public bool DaKhoa { get; set; } = false;
}