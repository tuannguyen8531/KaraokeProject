using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaraokeProject.Models
{
    public class DichVuThongKe
    {
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public int SoLuongTonDau { get; set; }
        public int SoLuongMua { get; set; }
        public int SoLuongBan { get; set; }
        public int SoLuongTonCuoi { get; set; }
        public string TenNhaCungCap { get; set; }
    }
}