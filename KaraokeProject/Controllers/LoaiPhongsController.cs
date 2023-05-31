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
    public class LoaiPhongsController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.LoaiPhongs.ToList().Select(n => n.MaLoaiPhong).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        // GET: LoaiPhongs
        public ActionResult Index()
        {
            return View(db.LoaiPhongs.ToList());
        }

        // GET: LoaiPhongs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhongs.Find(id);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Create
        public ActionResult Create()
        {
            ViewBag.MaLP = getMa("LP");
            return View();
        }

        // POST: LoaiPhongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiPhong,TenLoaiPhong,GiaPhong")] LoaiPhong loaiPhong)
        {
            string id = getMa("LP");
            if (ModelState.IsValid)
            {
                loaiPhong.MaLoaiPhong = id;
                db.LoaiPhongs.Add(loaiPhong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.ID = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhongs.Find(id);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // POST: LoaiPhongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiPhong,TenLoaiPhong,GiaPhong")] LoaiPhong loaiPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiPhong);
        }

        // GET: LoaiPhongs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhong loaiPhong = db.LoaiPhongs.Find(id);
            ViewBag.ID = loaiPhong.TenLoaiPhong;
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // POST: LoaiPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LoaiPhong loaiPhong = db.LoaiPhongs.Find(id);
            db.LoaiPhongs.Remove(loaiPhong);
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
