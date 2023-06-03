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
    public class KhachHangsController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.KhachHangs.ToList().Select(n => n.MaKhachHang).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        [HttpGet]
        public ActionResult Search(string ten = "", string sdt="", string diaChi="")
        {
            ViewBag.ten = ten;
            ViewBag.sdt = sdt;
            ViewBag.diaChi = diaChi;
            var khachHangs = db.KhachHangs.SqlQuery("TraCuuKhachHang N'" + ten + "', N'" + sdt + "', N'" + diaChi + "'");
            if (khachHangs.Count() == 0)
            {
                ViewBag.ThongBao = "Không có thông tin tìm kiếm.";
            }
            return View(khachHangs.ToList());
        }

        // GET: KhachHangs
        public ActionResult Index()
        {
            var khachHangs = db.KhachHangs.Include(k => k.TaiKhoan);
            return View(khachHangs.ToList());
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public ActionResult Create()
        {
            ViewBag.ID = getMa("KH");
            ViewBag.MaTaiKhoan = new SelectList(db.TaiKhoans, "MaTaiKhoan", "TenDangNhap");
            return View();
        }

        // POST: KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,TenKhachHang,DiaChi,SoDienThoai,MaTaiKhoan")] KhachHang khachHang)
        {
            string id = getMa("KH");
            if (ModelState.IsValid)
            {
                khachHang.MaKhachHang = id;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaTaiKhoan = new SelectList(db.TaiKhoans, "MaTaiKhoan", "TenNguoiDung", khachHang.MaTaiKhoan);
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            ViewBag.ID = khachHang.MaKhachHang;
            ViewBag.TK = khachHang.MaTaiKhoan;
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTaiKhoan = new SelectList(db.TaiKhoans, "MaTaiKhoan", "TenNguoiDung", khachHang.MaTaiKhoan);
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,TenKhachHang,DiaChi,SoDienThoai,MaTaiKhoan")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                khachHang.MaTaiKhoan = @ViewBag.TK;
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTaiKhoan = new SelectList(db.TaiKhoans, "MaTaiKhoan", "TenNguoiDung", khachHang.MaTaiKhoan);
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            ViewBag.ID = khachHang.TenKhachHang;
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
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
