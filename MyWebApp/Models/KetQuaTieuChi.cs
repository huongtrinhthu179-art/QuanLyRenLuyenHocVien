namespace MyWebApp.Models
{
    public class KetQuaTieuChi
    {
        public int Id { get; set; }

        public int HocVienId { get; set; }

        public int KyDanhGiaId { get; set; }

        public int TieuChiDanhGiaId { get; set; }

        public double DiemDat { get; set; }
        public double DiemToiDa { get; set; }
    }
}
