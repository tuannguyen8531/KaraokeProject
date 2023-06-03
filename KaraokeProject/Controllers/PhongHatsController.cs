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
    public class PhongHatsController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.PhongHats.ToList().Select(n => n.MaPhongHat).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        // GET: PhongHats
        public ActionResult Index()
        {
            var phongHats = db.PhongHats.Include(p => p.LoaiPhong1).Include(p => p.TrangThai1);
            return View(phongHats.ToList());
        }

        // GET: PhongHats/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHat phongHat = db.PhongHats.Find(id);
            if (phongHat == null)
            {
                return HttpNotFound();
            }
            return View(phongHat);
        }

        // GET: PhongHats/Create
        public ActionResult Create()
        {
            ViewBag.ID = getMa("PH");
            ViewBag.LoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            var trangThais = db.TrangThais.Where(tt => tt.MaTrangThai == "SS" || tt.MaTrangThai == "HD" || tt.MaTrangThai == "NC");
            ViewBag.TrangThai = new SelectList(trangThais, "MaTrangThai", "TenTrangThai");
            return View();
        }

        // POST: PhongHats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhongHat,TenPhongHat,LoaiPhong,TrangThai")] PhongHat phongHat)
        {
            string id = getMa("PH");
            if (ModelState.IsValid)
            {
                phongHat.MaPhongHat = id;
                db.PhongHats.Add(phongHat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phongHat.LoaiPhong);
            var trangThais = db.TrangThais.Where(tt => tt.MaTrangThai == "SS" || tt.MaTrangThai == "HD" || tt.MaTrangThai == "NC");
            ViewBag.TrangThai = new SelectList(trangThais, "MaTrangThai", "TenTrangThai", phongHat.TrangThai);
            return View(phongHat);
        }

        // GET: PhongHats/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHat phongHat = db.PhongHats.Find(id);
            ViewBag.ID = phongHat.MaPhongHat;
            if (phongHat == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phongHat.LoaiPhong);
            var trangThais = db.TrangThais.Where(tt => tt.MaTrangThai == "SS" || tt.MaTrangThai == "HD" || tt.MaTrangThai == "NC");
            ViewBag.TrangThai = new SelectList(trangThais, "MaTrangThai", "TenTrangThai", phongHat.TrangThai);
            return View(phongHat);
        }

        // POST: PhongHats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhongHat,TenPhongHat,LoaiPhong,TrangThai")] PhongHat phongHat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phongHat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", phongHat.LoaiPhong);
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai", phongHat.TrangThai);
            return View(phongHat);
        }

        // GET: PhongHats/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongHat phongHat = db.PhongHats.Find(id);
            ViewBag.ID = phongHat.TenPhongHat;
            if (phongHat == null)
            {
                return HttpNotFound();
            }
            return View(phongHat);
        }

        // POST: PhongHats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PhongHat phongHat = db.PhongHats.Find(id);
            db.PhongHats.Remove(phongHat);
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
