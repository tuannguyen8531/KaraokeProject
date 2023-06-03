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
    public class NhanViensController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMa(string loai)
        {
            var maMax = db.NhanViens.ToList().Select(n => n.MaNhanVien).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        [HttpGet]
        public ActionResult Search(string ten = "", string chucVu = "", string gioiTinh = "", string tuoiMin="", string tuoiMax = "", string luongMin = "", string luongMax = "")
        {
            ViewBag.ten = ten;
            var chucVus = db.TaiKhoans.Where(tk => tk.MaTaiKhoan == "TK02" || tk.MaTaiKhoan == "TK03" || tk.MaTaiKhoan == "TK04");
            ViewBag.chucVu = new SelectList(chucVus, "MaTaiKhoan", "TenNguoiDung");
            string tMin = tuoiMin, tMax = tuoiMax;
            string lMin = luongMin, lMax = luongMax;
            if (tuoiMin == "")
            {
                ViewBag.tuoiMin = "";
                tMin = "0";
            }
            else
            {
                ViewBag.tuoiMin = tuoiMin;
                tMin = tuoiMin;
            }
            if (tuoiMax == "")
            {
                ViewBag.tuoiMax = "";
                tMax = Int32.MaxValue.ToString();
            }
            else
            {
                ViewBag.tuoiMax = tuoiMax;
                tMax = tuoiMax;
            }
            if (luongMin == "")
            {
                ViewBag.luongMin = "";
                lMin = "0";
            }
            else
            {
                ViewBag.luongMin = luongMin;
                lMin = luongMin;
            }
            if (luongMax == "")
            {
                ViewBag.luongMax = "";
                lMax = Int32.MaxValue.ToString();
            }
            else
            {
                ViewBag.luongMax = luongMax;
                lMax = luongMax;
            }
            var nhanViens = db.NhanViens.SqlQuery("TraCuuNhanVien N'" + ten + "', N'" + chucVu + "', N'" + gioiTinh + "', N'" + tMin + "', N'" + tMax + "', N'" + lMin + "', N'" + lMax + "'");
            if (nhanViens.Count() == 0)
            {
                ViewBag.ThongBao = "Không có thông tin tìm kiếm.";
            }
            return View(nhanViens.ToList());
        }

        // GET: NhanViens
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Include(n => n.TaiKhoan);
            return View(nhanViens.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.ID = getMa("NV");
            var taiKhoans = db.TaiKhoans.Where(tk => tk.MaTaiKhoan == "TK02" || tk.MaTaiKhoan == "TK03" || tk.MaTaiKhoan == "TK04").ToList();
            ViewBag.MaTaiKhoan = new SelectList(taiKhoans, "MaTaiKhoan", "TenNguoiDung");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhanVien,TenNhanVien,NgaySinh,GioiTinh,Luong,MaTaiKhoan")] NhanVien nhanVien)
        {
            string id = getMa("NV");
            if (ModelState.IsValid)
            {
                nhanVien.MaNhanVien = id;
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var taiKhoans = db.TaiKhoans.Where(t => int.Parse(t.MaTaiKhoan.Substring(2)) >= 2 && int.Parse(t.MaTaiKhoan.Substring(2)) <= 4).ToList();
            ViewBag.MaTaiKhoan = new SelectList(taiKhoans, "MaTaiKhoan", "TenNguoiDung", nhanVien.MaTaiKhoan);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            ViewBag.ID = nhanVien.MaNhanVien;
            Session["MaTKNV"] = nhanVien.MaTaiKhoan;
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            var taiKhoans = db.TaiKhoans.Where(tk => tk.MaTaiKhoan == "TK02" || tk.MaTaiKhoan == "TK03" || tk.MaTaiKhoan == "TK04").ToList();
            ViewBag.MaTaiKhoan = new SelectList(taiKhoans, "MaTaiKhoan", "TenNguoiDung");
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhanVien,TenNhanVien,NgaySinh,GioiTinh,Luong,MaTaiKhoan")] NhanVien nhanVien)
        {
            string id = (string)Session["MaTKNV"];
            if (ModelState.IsValid)
            {
                nhanVien.MaTaiKhoan = id;
                Session.Remove("MaTKNV");
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTaiKhoan = new SelectList(db.TaiKhoans, "MaTaiKhoan", "TenNguoiDung", nhanVien.MaTaiKhoan);
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            ViewBag.ID = nhanVien.TenNhanVien;
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
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
