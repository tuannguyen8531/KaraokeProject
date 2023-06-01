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
    public class DatPhongsController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.DatPhongs.ToList().Select(n => n.MaDatPhong).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        // GET: DatPhongs
        public ActionResult Index()
        {
            var datPhongs = db.DatPhongs.Include(d => d.KhachHang).Include(d => d.PhongHat).Include(d => d.TrangThai1);
            var dangHDs = datPhongs.Where(d => d.TrangThai == "HD");
            return View(dangHDs.ToList());
        }

        // GET: DatPhongs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatPhong datPhong = db.DatPhongs.Find(id);
            if (datPhong == null)
            {
                return HttpNotFound();
            }
            return View(datPhong);
        }

        // GET: DatPhongs/Create
        public ActionResult Create()
        {
            ViewBag.ID = getMa("DP");
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang");
            var phongKDs = db.PhongHats.Where(p => p.TrangThai == "SS");
            ViewBag.MaPhongHat = new SelectList(phongKDs, "MaPhongHat", "TenPhongHat");
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai");
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DateTime currentDay = DateTime.Now;
            ViewBag.CurrentTime = currentTime.ToString(@"hh\:mm");
            ViewBag.CurrentDay = currentDay.ToString("dd/MM/yyyy");
            return View();
        }

        // POST: DatPhongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDatPhong,GioVao,NgayDat,MaKhachHang,MaPhongHat,TrangThai")] DatPhong datPhong)
        {
            string id = getMa("DP");
            if (ModelState.IsValid)
            {
                datPhong.TrangThai = "HD";
                datPhong.MaDatPhong = id;
                datPhong.GioVao = DateTime.Now.TimeOfDay;
                datPhong.NgayDat = DateTime.Now;
                db.DatPhongs.Add(datPhong);
                db.SaveChanges();

                var phongHat = db.PhongHats.FirstOrDefault(ph => ph.MaPhongHat == datPhong.MaPhongHat);
                if (phongHat != null)
                {
                    phongHat.TrangThai = datPhong.TrangThai;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            ViewBag.MaPhongHat = new SelectList(db.PhongHats, "MaPhongHat", "TenPhongHat", datPhong.MaPhongHat);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai", datPhong.TrangThai);
            return View(datPhong);
        }

        // GET: DatPhongs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatPhong datPhong = db.DatPhongs.Find(id);
            if (datPhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            ViewBag.MaPhongHat = new SelectList(db.PhongHats, "MaPhongHat", "TenPhongHat", datPhong.MaPhongHat);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai", datPhong.TrangThai);
            return View(datPhong);
        }

        // POST: DatPhongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDatPhong,GioVao,NgayDat,MaKhachHang,MaPhongHat,TrangThai")] DatPhong datPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", datPhong.MaKhachHang);
            ViewBag.MaPhongHat = new SelectList(db.PhongHats, "MaPhongHat", "TenPhongHat", datPhong.MaPhongHat);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai", datPhong.TrangThai);
            return View(datPhong);
        }

        // GET: DatPhongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatPhong datPhong = db.DatPhongs.Find(id);
            var phongHat = db.PhongHats.FirstOrDefault(ph => ph.MaPhongHat == datPhong.MaPhongHat);
            if (phongHat != null)
            {
                phongHat.TrangThai = "SS";
                db.SaveChanges();
            }
            if (datPhong == null)
            {
                return HttpNotFound();
            }
            return View(datPhong);
        }

        // POST: DatPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DatPhong datPhong = db.DatPhongs.Find(id);
            db.DatPhongs.Remove(datPhong);
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
