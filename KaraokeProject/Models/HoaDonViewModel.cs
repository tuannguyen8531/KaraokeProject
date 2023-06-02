using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaraokeProject.Models
{
    public class HoaDonViewModel
    {
        public string MaNhanVien { get; set; }
        public string MaDatPhong { get; set; }
        public TimeSpan GioRa { get; set; }
        public System.DateTime NgayLapHD { get; set; }
        public List<ChiTietHoaDonViewModel> ChiTietHoaDon { get; set; }

        
    }

    public class ChiTietHoaDonViewModel
    {
        public string MaDichVu { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
    }

}