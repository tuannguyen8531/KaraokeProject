using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KaraokeProject.Models;

namespace KaraokeProject.Controllers
{
    public class TrangChuController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        public ActionResult Index()
        {
            int tongDT = 0;
            int tongCP = 0;
            List<HoaDon> danhSachHoaDon = db.HoaDons.ToList();
            foreach (var hoaDon in danhSachHoaDon)
            {
                int tien = (int)hoaDon.TongTien;
                tongDT += tien;
            }
            List<PhieuNhap> danhSachPhieuNhap = db.PhieuNhaps.ToList();
            foreach (var phieuNhap in danhSachPhieuNhap)
            {
                int tien = (int)phieuNhap.TongTien;
                tongCP += tien;
            }
            ViewBag.TongDoanhThu = tongDT;
            ViewBag.TongSoPhong = db.PhongHats.Where(t => t.TrangThai == "SS").Count();
            ViewBag.TongChiPhi = tongCP;
            ViewBag.TongHoatDong = db.PhongHats.Where(t => t.TrangThai == "HD").Count();
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}
