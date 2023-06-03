using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KaraokeProject.Models;

namespace KaraokeProject.Controllers
{
    public class TaiKhoansController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        public bool checkUser(string username, string password)
        {
            var result = db.TaiKhoans.Where(x => x.TenDangNhap == username && x.MatKhau == password).ToList();
            if (result.Count() > 0)
            {
                Session["NguoiDung"] = result.First().TenNguoiDung;
                Session["PhanQuyen"] = result.First().PhanQuyen1.TenPhanQuyen;
                return true;
            }
            Session["NguoiDung"] = null;
            Session["PhanQuyen"] = null;
            return false;
        }

        public ActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string id, string oldpass, string newpass)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(newpass))
            {
                ViewBag.Trong = "Mật khẩu không được để trống";
                return View(taiKhoan);
            }

            if (oldpass != taiKhoan.MatKhau)
            {
                ViewBag.SaiMatKhau = "Sai mật khẩu";
                return View(taiKhoan);
            }

            taiKhoan.MatKhau = newpass;
            db.Entry(taiKhoan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        string getMa(string loai)
        {
            var maMax = db.TaiKhoans.ToList().Select(n => n.MaTaiKhoan).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                if (checkUser(tk.TenDangNhap, tk.MatKhau))
                {
                    var result = db.TaiKhoans.Where(x => x.TenDangNhap == tk.TenDangNhap).ToList();
                    var temp = result.First().PhanQuyen;
                    FormsAuthentication.SetAuthCookie(tk.TenDangNhap, true);
                    /*if (temp == "QL") return RedirectToAction("Index", "DichVus");*/
                    return RedirectToAction("Index", "TrangChu");

                }
                else ModelState.AddModelError("", "Tên đăng nhập hoặc tài khoản không đúng.");
                ViewBag.ThongBao = "Tên đăng nhập hoặc tài khoản không đúng.";
            }
            return View(tk);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "TaiKhoans");
        }

        // GET: TaiKhoans
        public ActionResult Index()
        {
            var taiKhoans = db.TaiKhoans.Include(t => t.PhanQuyen1);
            return View(taiKhoans.ToList());
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            ViewBag.ID = getMa("TK");
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "MaPhanQuyen", "TenPhanQuyen");
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTaiKhoan,TenNguoiDung,TenDangNhap,MatKhau,PhanQuyen")] TaiKhoan taiKhoan)
        {
            string id = getMa("TK");
            if (ModelState.IsValid)
            {
                taiKhoan.PhanQuyen = "NV";
                taiKhoan.MaTaiKhoan = id;
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "MaPhanQuyen", "TenPhanQuyen", taiKhoan.PhanQuyen);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "MaPhanQuyen", "TenPhanQuyen", taiKhoan.PhanQuyen);
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTaiKhoan,TenNguoiDung,TenDangNhap,MatKhau,PhanQuyen")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhanQuyen = new SelectList(db.PhanQuyens, "MaPhanQuyen", "TenPhanQuyen", taiKhoan.PhanQuyen);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            ViewBag.ID = taiKhoan.TenNguoiDung;
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
