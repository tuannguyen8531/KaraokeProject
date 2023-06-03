using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KaraokeProject.Models;

namespace KaraokeProject.Controllers
{
    public class HoaDonsController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.HoaDons.ToList().Select(n => n.MaHoaDon).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        // GET: HoaDons
        public ActionResult Index()
        {
            var hoaDons = db.HoaDons.Include(h => h.DatPhong).Include(h => h.NhanVien);
            return View(hoaDons.ToList());
        }

        // GET: HoaDons/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHoaDon = hoaDon.MaHoaDon;
            ViewBag.Phong = hoaDon.DatPhong.PhongHat.TenPhongHat;
            ViewBag.KhachHang = hoaDon.DatPhong.KhachHang.TenKhachHang;
            ViewBag.NhanVien = hoaDon.NhanVien.TenNhanVien;
            ViewBag.TongTien = hoaDon.TongTien;
            ViewBag.GioVao = hoaDon.DatPhong.GioVao;
            ViewBag.GioRa = hoaDon.GioRa;
            ViewBag.NgayLap = hoaDon.NgayLapHD;
            var cthds = db.ChiTietHoaDons.Where(t => t.MaHoaDon == hoaDon.MaHoaDon);
            return View(cthds.ToList());
        }

        // GET: HoaDons/Create
        public ActionResult Create(string id)
        {
            ViewBag.ID = getMa("HD");
            ViewBag.MaDatPhong = id;
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien");
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DateTime currentDay = DateTime.Now;
            var dp = db.DatPhongs.FirstOrDefault(t => t.MaDatPhong == id);
            TimeSpan thoiGian = (TimeSpan)(currentTime - dp.GioVao);
            int phut = (int)thoiGian.TotalMinutes;
            var ph = db.PhongHats.FirstOrDefault(t => t.MaPhongHat == dp.MaPhongHat);
            var gia = db.LoaiPhongs.FirstOrDefault(t => t.MaLoaiPhong == ph.LoaiPhong).GiaPhong;
            ViewBag.Total = phut * gia;
            ViewBag.CurrentTime = currentTime.ToString(@"hh\:mm");
            ViewBag.CurrentDay = currentDay.ToString("dd/MM/yyyy");
            return View();
        }

        // POST: HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, [Bind(Include = "MaHoaDon,NgayLapHD,MaNhanVien,MaDatPhong,TongTien,GioRa")] HoaDon hoaDon)
        {
            string ma = id;
            string idHD = getMa("HD");
            if (ModelState.IsValid)
            {
                hoaDon.MaHoaDon = idHD;
                hoaDon.GioRa = DateTime.Now.TimeOfDay;
                hoaDon.NgayLapHD = DateTime.Now;
                hoaDon.MaDatPhong = ma;
                var dp = db.DatPhongs.FirstOrDefault(t => t.MaDatPhong == ma);
                TimeSpan thoiGian = (TimeSpan)(hoaDon.GioRa - dp.GioVao);
                int phut = (int)thoiGian.TotalMinutes;
                var ph = db.PhongHats.FirstOrDefault(t => t.MaPhongHat == dp.MaPhongHat);
                var gia = db.LoaiPhongs.FirstOrDefault(t => t.MaLoaiPhong == ph.LoaiPhong).GiaPhong;
                hoaDon.TongTien = phut * gia;
                db.HoaDons.Add(hoaDon);
                dp.TrangThai = "HT";
                ph.TrangThai = "SS";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDatPhong = Request.Form["id"];
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", hoaDon.MaNhanVien);
            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDatPhong = new SelectList(db.DatPhongs, "MaDatPhong", "MaKhachHang", hoaDon.MaDatPhong);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", hoaDon.MaNhanVien);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHoaDon,NgayLapHD,MaNhanVien,MaDatPhong,TongTien,GioRa")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDatPhong = new SelectList(db.DatPhongs, "MaDatPhong", "MaKhachHang", hoaDon.MaDatPhong);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", hoaDon.MaNhanVien);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HoaDon hoaDon = db.HoaDons.Find(id);
            db.HoaDons.Remove(hoaDon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
