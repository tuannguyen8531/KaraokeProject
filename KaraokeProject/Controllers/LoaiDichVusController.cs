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
    public class LoaiDichVusController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        // GET: LoaiDichVus
        public ActionResult Index()
        {
            return View(db.LoaiDichVus.ToList());
        }

        // GET: LoaiDichVus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDichVu loaiDichVu = db.LoaiDichVus.Find(id);
            if (loaiDichVu == null)
            {
                return HttpNotFound();
            }
            return View(loaiDichVu);
        }

        // GET: LoaiDichVus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiDichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiDV,TenLoaiDV")] LoaiDichVu loaiDichVu)
        {
            if (ModelState.IsValid)
            {
                db.LoaiDichVus.Add(loaiDichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiDichVu);
        }

        // GET: LoaiDichVus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDichVu loaiDichVu = db.LoaiDichVus.Find(id);
            if (loaiDichVu == null)
            {
                return HttpNotFound();
            }
            return View(loaiDichVu);
        }

        // POST: LoaiDichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiDV,TenLoaiDV")] LoaiDichVu loaiDichVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiDichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiDichVu);
        }

        // GET: LoaiDichVus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDichVu loaiDichVu = db.LoaiDichVus.Find(id);
            if (loaiDichVu == null)
            {
                return HttpNotFound();
            }
            return View(loaiDichVu);
        }

        // POST: LoaiDichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LoaiDichVu loaiDichVu = db.LoaiDichVus.Find(id);
            db.LoaiDichVus.Remove(loaiDichVu);
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
