namespace MyWebApp.Models;

public class TieuChiDanhGia
{
    public int Id { get; set; }

    public string TenTieuChi { get; set; } = "";

    public string MoTa { get; set; } = "";

    public double DiemToiDa { get; set; }

    public int ThuTu { get; set; }

    public bool DangSuDung { get; set; } = true;
}