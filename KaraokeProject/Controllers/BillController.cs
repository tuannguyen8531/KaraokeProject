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
    public class BillController : Controller
    {
        private QLKaraokeEntities db = new QLKaraokeEntities();

        static public List<ChiTietHoaDonViewModel> chiTietHoaDons = new List<ChiTietHoaDonViewModel>();

        string getMa(string loai)
        {
            var maMax = db.HoaDons.ToList().Select(n => n.MaHoaDon).Max();
            int ma = int.Parse(maMax.Substring(2)) + 1;
            string type = String.Concat("0", ma.ToString());
            return loai + type.Substring(ma.ToString().Length - 1);
        }

        public ActionResult GetThongTinDichVu(string maDichVu)
        {
            var dichVu = db.DichVus.FirstOrDefault(dv => dv.MaDichVu == maDichVu);
            var thongTinDichVu = new
            {
                TenDichVu = dichVu.TenDichVu,
                DonVi = dichVu.DonViTinh1.TenDonVi,
                DonGia = dichVu.DonGia
            };
            return Json(thongTinDichVu, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateBill(string id)
        {
            ViewBag.ID = getMa("HD");
            Session["MaDatPhong"] = id;
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien");
            ViewBag.DichVu = new SelectList(db.DichVus, "MaDichVu", "MaDichVu");
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DateTime currentDay = DateTime.Now;
            var dp = db.DatPhongs.FirstOrDefault(t => t.MaDatPhong == id);
            ViewBag.MaDatPhong = dp.PhongHat.TenPhongHat;
            TimeSpan thoiGian = (TimeSpan)(currentTime - dp.GioVao);
            int phut = (int)thoiGian.TotalMinutes;
            var ph = db.PhongHats.FirstOrDefault(t => t.MaPhongHat == dp.MaPhongHat);
            var gia = db.LoaiPhongs.FirstOrDefault(t => t.MaLoaiPhong == ph.LoaiPhong).GiaPhong;
            ViewBag.Hours = phut * gia;
            Session["TienGio"] = phut * gia;
            Session["TongTien"] = Session["TienGio"];
            ViewBag.CurrentTime = currentTime.ToString(@"hh\:mm");
            ViewBag.CurrentDay = currentDay.ToString("dd/MM/yyyy");

            HoaDonViewModel viewModel = new HoaDonViewModel();
            viewModel.GioRa = currentTime;
            viewModel.NgayLapHD = currentDay;
            viewModel.MaDatPhong = id;
            if (Session["ChiTietHoaDon"] == null)
            {
                viewModel.ChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
                Session["ChiTietHoaDon"] = viewModel.ChiTietHoaDon;
            }
            else
            {
                int tongTien = (int)Session["TienGio"];
                viewModel.ChiTietHoaDon = (List<ChiTietHoaDonViewModel>)Session["ChiTietHoaDon"];
                foreach (var chiTietHoaDon in viewModel.ChiTietHoaDon)
                {
                    int tien = chiTietHoaDon.DonGia * chiTietHoaDon.SoLuong;
                    tongTien += tien;
                }
                Session["TongTien"] = tongTien;
                ViewBag.Total = Session["TongTien"];

            }
            ViewBag.Total = Session["TongTien"];
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBill(HoaDonViewModel viewModel)
        {
            string id = (string)Session["MaDatPhong"];
            if (ModelState.IsValid)
            {
                var dp = db.DatPhongs.FirstOrDefault(t => t.MaDatPhong == id);
                var ph = db.PhongHats.FirstOrDefault(t => t.MaPhongHat == dp.MaPhongHat);

                HoaDon hoaDon = new HoaDon();
                hoaDon.MaHoaDon = getMa("HD");
                hoaDon.NgayLapHD = DateTime.Now;
                hoaDon.MaNhanVien = viewModel.MaNhanVien;
                hoaDon.MaDatPhong = id;
                hoaDon.GioRa = DateTime.Now.TimeOfDay;
                hoaDon.TongTien = (int)Session["TienGio"];
                List<ChiTietHoaDonViewModel> listCTHD = (List<ChiTietHoaDonViewModel>)Session["ChiTietHoaDon"];
                int dsDV = listCTHD.Count();
                if(dsDV!=0)
                {
                    foreach (var chiTietHoaDon in viewModel.ChiTietHoaDon)
                    {
                        int tien = chiTietHoaDon.DonGia * chiTietHoaDon.SoLuong;
                        hoaDon.TongTien += tien;
                    }
                    db.HoaDons.Add(hoaDon);
                    db.SaveChanges();

                    
                    foreach (var chiTietHoaDon in viewModel.ChiTietHoaDon)
                    {
                        ChiTietHoaDon chiTiet = new ChiTietHoaDon();
                        chiTiet.MaHoaDon = hoaDon.MaHoaDon;
                        chiTiet.MaDichVu = chiTietHoaDon.MaDichVu;
                        chiTiet.SoLuongBan = chiTietHoaDon.SoLuong;
                        chiTiet.DonGiaBan = chiTietHoaDon.DonGia;
                        

                        db.ChiTietHoaDons.Add(chiTiet);
                        var dichVu = db.DichVus.FirstOrDefault(dv => dv.MaDichVu == chiTiet.MaDichVu);
                        int soLuongBan = chiTiet.SoLuongBan ?? 0;
                        dichVu.SoLuongTon -= soLuongBan;
                        db.Entry(dichVu).State = EntityState.Modified;
                    }
                    dp.TrangThai = "HT";
                    ph.TrangThai = "SS";
                    Session.Remove("MaDatPhong");
                    Session.Remove("TienGio");
                    Session.Remove("ChiTietHoaDon");
                    db.SaveChanges();
                    return RedirectToAction("Index", "HoaDons");
                }
                db.HoaDons.Add(hoaDon);
                db.SaveChanges();
                dp.TrangThai = "HT";
                ph.TrangThai = "SS";
                Session.Remove("MaDatPhong");
                Session.Remove("TienGio");
                db.SaveChanges();
                return RedirectToAction("Index", "DatPhongs");
            }

            return View(viewModel);
        }

        public ActionResult AddNewCTHD()
        {
            string id = (string)Session["MaDatPhong"];
            List<ChiTietHoaDonViewModel> listCTHD;
            if (Session["ChiTietHoaDon"] == null)
            {
                listCTHD = new List<ChiTietHoaDonViewModel>();
            }
            else
            {
                listCTHD = (List<ChiTietHoaDonViewModel>)Session["ChiTietHoaDon"];
            }

            ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
            chiTietHoaDon.MaDichVu = "";
            chiTietHoaDon.SoLuong = 1;
            chiTietHoaDon.DonGia = 0;
            listCTHD.Add(chiTietHoaDon);

            Session["ChiTietHoaDon"] = listCTHD;

            return RedirectToAction("CreateBill", new RouteValueDictionary { { "id", id } });
        }

        public ActionResult RemoveCTHD(int index)
        {
            string id = (string)Session["MaDatPhong"];
            List<ChiTietHoaDonViewModel> listCTHD;
            if (Session["ChiTietHoaDon"] == null)
            {
                listCTHD = new List<ChiTietHoaDonViewModel>();
            }
            else
            {
                listCTHD = (List<ChiTietHoaDonViewModel>)Session["ChiTietHoaDon"];
            }


            listCTHD.RemoveAt(index);

            Session["ChiTietHoaDon"] = listCTHD;

            return RedirectToAction("CreateBill", new RouteValueDictionary { { "id", id } });
        }


        [HttpPost]
        public ActionResult AddCTHDIntoList(string selectedDichVu, int selectedSoLuong, int selectedDonGia, int index)
        {
            List<ChiTietHoaDonViewModel> listCTHD;
            if (Session["ChiTietHoaDon"] == null)
            {
                listCTHD = new List<ChiTietHoaDonViewModel>();
            }
            else
            {
                listCTHD = (List<ChiTietHoaDonViewModel>)Session["ChiTietHoaDon"];
            }
            ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
            chiTietHoaDon.MaDichVu = selectedDichVu;
            chiTietHoaDon.SoLuong = selectedSoLuong;
            chiTietHoaDon.DonGia = selectedDonGia;
            listCTHD[index] = chiTietHoaDon;
            Session["ChiTietHoaDon"] = listCTHD;
            int temp = (int)Session["TongTien"] + (selectedDonGia * selectedSoLuong);
            Session["TongTien"] = temp;

            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult ThongKeTonKho(DateTime? ngayBatDau, DateTime? ngayKetThuc, string loaiDV = "")
        {
            DateTime start = (DateTime)(ngayBatDau ?? SqlDateTime.MinValue);
            DateTime end = (DateTime)(ngayKetThuc ?? SqlDateTime.MaxValue);
            ViewBag.loaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            List<DichVuThongKe> dichVus = db.Database.SqlQuery<DichVuThongKe>(
                "EXEC ThongKeTonKho @NgayBatDau, @NgayKetThuc, @LoaiDichVu",
                new SqlParameter("@NgayBatDau", ngayBatDau ?? SqlDateTime.MinValue),
                new SqlParameter("@NgayKetThuc", ngayKetThuc ?? SqlDateTime.MaxValue),
                new SqlParameter("@LoaiDichVu", string.IsNullOrEmpty(loaiDV) ? DBNull.Value : (object)loaiDV)
            ).ToList();
            int tongMua = 0;
            int tongBan = 0;
            int tongTien = 0;
            int tongPhieuNhap = 0;
            foreach (var item in dichVus)
            {
                tongMua += item.SoLuongMua;
                tongBan += item.SoLuongBan;
            }
            List<PhieuNhap> danhSachPhieuNhap = db.PhieuNhaps.Where(hd => hd.NgayLapPN >= start && hd.NgayLapPN <= end).ToList();
            foreach (var phieuNhap in danhSachPhieuNhap)
            {
                int tien = (int)phieuNhap.TongTien;
                tongTien += tien;
                tongPhieuNhap++;
            }
            ViewBag.TongTien = tongTien;
            ViewBag.TongPhieuNhap = tongPhieuNhap;
            ViewBag.TongMua = tongMua;
            ViewBag.TongBan = tongBan;
            return View(dichVus);
        }

        [HttpGet]
        public ActionResult ThongKeDoanhThu(DateTime? ngayBatDau, DateTime? ngayKetThuc)
        {
            DateTime start = (DateTime)(ngayBatDau ?? SqlDateTime.MinValue);
            DateTime end = (DateTime)(ngayKetThuc ?? SqlDateTime.MaxValue);
            int tongTien = 0;
            int tongHoaDon = 0;
            List<HoaDon> danhSachHoaDon = db.HoaDons.Where(hd => hd.NgayLapHD >= start && hd.NgayLapHD <= end).ToList();
            int soLuongKhachHang = 0;
            HashSet<string> danhSachMaKhachHang = new HashSet<string>();
            HashSet<string> maPhongDaSuDung = new HashSet<string>();
            int soLuongPhongSuDung = 0;
            foreach (var hoaDon in danhSachHoaDon)
            {
                int tien = (int)hoaDon.TongTien;
                tongTien += tien;
                tongHoaDon++;
                if (hoaDon.DatPhong != null && hoaDon.DatPhong.MaKhachHang == "KH01")
                {
                    soLuongKhachHang++;
                }
                else if (hoaDon.DatPhong != null)
                {
                    danhSachMaKhachHang.Add(hoaDon.DatPhong.MaKhachHang);
                }
                if(hoaDon != null && hoaDon.DatPhong != null)
                {
                    string maPhong = hoaDon.DatPhong.MaPhongHat;
                    if (!maPhongDaSuDung.Contains(maPhong))
                    {
                        maPhongDaSuDung.Add(maPhong);
                        soLuongPhongSuDung++;
                    }
                }    
            }
            soLuongKhachHang += danhSachMaKhachHang.Count;
            ViewBag.TongPhong = soLuongPhongSuDung;
            ViewBag.TongKhachHang = soLuongKhachHang;
            ViewBag.TongTien = tongTien;
            ViewBag.TongHoaDon = tongHoaDon;
            return View(danhSachHoaDon);
        }
    }

}
