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
    public class DichVusController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        string getMaDichVu(string loai)
        {
            var maMax = db.DichVus.ToList().Select(n => n.MaDichVu).Max();
            int maDV = int.Parse(maMax.Substring(2)) + 1;
            string DV = String.Concat("0", maDV.ToString());
            return loai + DV.Substring(maDV.ToString().Length - 1);
        }

        [HttpGet]
        public ActionResult Search(string ten = "", string loai = "", string nhaCC = "", string giaMin = "", string giaMax = "", string soLuongMin = "", string soLuongMax = "")
        {
            string gMin = giaMin, gMax = giaMax;
            string sMin = soLuongMin, sMax = soLuongMax;
            ViewBag.tenMH = ten;
            if (giaMin == "")
            {
                ViewBag.giaMin = "";
                gMin = "0";
            }
            else
            {
                ViewBag.giaMin = giaMin;
                gMin = giaMin;
            }
            if (giaMax == "")
            {
                ViewBag.giaMax = "";
                gMax = Int32.MaxValue.ToString();
            }
            else
            {
                ViewBag.giaMax = giaMax;
                gMax = giaMax;
            }
            if (soLuongMin == "")
            {
                ViewBag.soLuongMin = "";
                sMin = "0";
            }
            else
            {
                ViewBag.soLuongMin = soLuongMin;
                sMin = soLuongMin;
            }
            if (soLuongMax == "")
            {
                ViewBag.soLuongMax = "";
                sMax = Int32.MaxValue.ToString();
            }
            else
            {
                ViewBag.soLuongMax = soLuongMax;
                sMax = soLuongMax;
            }
            ViewBag.loai = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            ViewBag.nhaCC = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            var matHangs = db.DichVus.SqlQuery("TraCuuDichVu N'" + ten + "', '" + loai + "', '" + nhaCC + "', N'" + gMin + "', N'" + gMax + "', N'" + sMin + "', N'" + sMax + "'");
            if (matHangs.Count() == 0)
            {
                ViewBag.ThongBao = "Không có thông tin tìm kiếm.";
            }
            return View(matHangs.ToList());
        }

        // GET: DichVus
        public ActionResult Index()
        {
            var dichVus = db.DichVus.Include(d => d.DonViTinh1).Include(d => d.LoaiDichVu1).Include(d => d.NhaCungCap1);
            return View(dichVus.ToList());
        }

        // GET: DichVus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DichVu dichVu = db.DichVus.Find(id);
            if (dichVu == null)
            {
                return HttpNotFound();
            }
            return View(dichVu);
        }

        // GET: DichVus/Create
        public ActionResult Create()
        {
            ViewBag.MaDV = getMaDichVu("SP");
            ViewBag.DonViTinh = new SelectList(db.DonViTinhs, "MaDonVi", "TenDonVi");
            ViewBag.LoaiDichVu = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            return View();
        }

        // POST: DichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDichVu,TenDichVu,LoaiDichVu,DonViTinh,DonGia,SoLuongTon,NhaCungCap")] DichVu dichVu)
        {
            string maDV = getMaDichVu("SP");
            if (ModelState.IsValid)
            {
                dichVu.MaDichVu = maDV;
                db.DichVus.Add(dichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DonViTinh = new SelectList(db.DonViTinhs, "MaDonVi", "TenDonVi", dichVu.DonViTinh);
            ViewBag.LoaiDichVu = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", dichVu.LoaiDichVu);
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", dichVu.NhaCungCap);
            return View(dichVu);
        }

        // GET: DichVus/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.MaDV = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DichVu dichVu = db.DichVus.Find(id);
            if (dichVu == null)
            {
                return HttpNotFound();
            }
            ViewBag.DonViTinh = new SelectList(db.DonViTinhs, "MaDonVi", "TenDonVi", dichVu.DonViTinh);
            ViewBag.LoaiDichVu = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", dichVu.LoaiDichVu);
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", dichVu.NhaCungCap);
            return View(dichVu);
        }

        // POST: DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDichVu,TenDichVu,LoaiDichVu,DonViTinh,DonGia,SoLuongTon,NhaCungCap")] DichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dichVu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DonViTinh = new SelectList(db.DonViTinhs, "MaDonVi", "TenDonVi", dichVu.DonViTinh);
            ViewBag.LoaiDichVu = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", dichVu.LoaiDichVu);
            ViewBag.NhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", dichVu.NhaCungCap);
            return View(dichVu);
        }

        // GET: DichVus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DichVu dichVu = db.DichVus.Find(id);
            ViewBag.MaDV = dichVu.MaDichVu;
            ViewBag.TenDV = dichVu.TenDichVu;
            ViewBag.LoaiDV = dichVu.LoaiDichVu1.TenLoaiDV;
            ViewBag.NhaCungCap = dichVu.NhaCungCap1.TenNhaCungCap;
            ViewBag.DonVi = dichVu.DonViTinh1.TenDonVi;
            ViewBag.DonGia = dichVu.DonGia;
            ViewBag.SoLuongTon = dichVu.SoLuongTon;
            if (dichVu == null)
            {
                return HttpNotFound();
            }
            return View(dichVu);
        }

        // POST: DichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DichVu dichVu = db.DichVus.Find(id);
            db.DichVus.Remove(dichVu);
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
