﻿USE MASTER

GO
-- Tạo cơ sở dữ liệu --
CREATE DATABASE QLKaraoke
GO
USE QLKaraoke
GO


-- Tạo các bảng dữ liệu --
CREATE TABLE ThamSo
(
	MaThamSo NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenThamSo NVARCHAR(50) NOT NULL,
	DonVi INT NOT NULL,
	GiaTri INT NOT NULL,
	TinhTrang BIT DEFAULT(1),
)
GO
CREATE TABLE LoaiDichVu
(
	MaLoaiDV NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenLoaiDV NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE DonViTinh
(
	MaDonVi NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenDonVi NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE NhaCungCap
(
	MaNhaCungCap NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenNhaCungCap NVARCHAR(50) NOT NULL,
	SoDienThoai NVARCHAR(20),
	DiaChi NVARCHAR(1000)
)
GO
CREATE TABLE LoaiPhong
(
	MaLoaiPhong NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenLoaiPhong NVARCHAR(50) NOT NULL,
	GiaPhong INT NOT NULL
)
GO
CREATE TABLE TrangThai
(
	MaTrangThai NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenTrangThai NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE DichVu
(
	MaDichVu NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenDichVu NVARCHAR(50) NOT NULL,
	LoaiDichVu NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES LoaiDichVu(MaLoaiDV)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	DonViTinh NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES DonViTinh(MaDonVi)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	DonGia INT NOT NULL,
	SoLuongTon INT NOT NULL,
	NhaCungCap NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES NhaCungCap(MaNhaCungCap)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
CREATE TABLE PhongHat
(
	MaPhongHat NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenPhongHat NVARCHAR(50) NOT NULL,
	LoaiPhong NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES LoaiPhong(MaLoaiPhong)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	TrangThai NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES TrangThai(MaTrangThai)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
CREATE TABLE PhanQuyen
(
	MaPhanQuyen NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenPhanQuyen NVARCHAR(50) NOT NULL
)
GO
CREATE TABLE TaiKhoan
(
	MaTaiKhoan NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenNguoiDung NVARCHAR(50) NOT NULL,
	TenDangNhap NVARCHAR(50) NOT NULL,
	MatKhau NVARCHAR(50) NOT NULL,
	PhanQuyen NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES PhanQuyen(MaPhanQuyen)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
CREATE TABLE KhachHang
(
	MaKhachHang NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenKhachHang NVARCHAR(50) NOT NULL,
	DiaChi NVARCHAR(100),
	SoDienThoai NVARCHAR(20),
	MaTaiKhoan NVARCHAR(9) FOREIGN KEY REFERENCES TaiKhoan(MaTaiKhoan)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
CREATE TABLE DatPhong
(
	MaDatPhong NVARCHAR(9) NOT NULL PRIMARY KEY,
	GioVao TIME NOT NULL,
	NgayDat DATE,
	MaKhachHang NVARCHAR(9) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
	MaPhongHat NVARCHAR(9) FOREIGN KEY REFERENCES PhongHat(MaPhongHat),
	TrangThai NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES TrangThai(MaTrangThai)
	ON UPDATE CASCADE
	ON DELETE CASCADE

)
GO
CREATE TABLE NhanVien
(
	MaNhanVien NVARCHAR(9) NOT NULL PRIMARY KEY,
	TenNhanVien NVARCHAR(50) NOT NULL,
	NgaySinh DATE,
	GioiTinh BIT DEFAULT(1),
	Luong INT,
	MaTaiKhoan NVARCHAR(9) FOREIGN KEY REFERENCES TaiKhoan(MaTaiKhoan)
	ON UPDATE CASCADE
	ON DELETE CASCADE
)
GO
CREATE TABLE HoaDon
(
	MaHoaDon NVARCHAR(9) NOT NULL PRIMARY KEY,
	NgayLapHD DATE NOT NULL,
	MaNhanVien NVARCHAR(9) FOREIGN KEY REFERENCES NhanVien(MaNhanVien),
	MaDatPhong NVARCHAR(9) FOREIGN KEY REFERENCES DatPhong(MaDatPhong),
	TongTien INT,
	GioRa TIME NOT NULL
)
GO
CREATE TABLE PhieuNhap
(
	MaPhieuNhap NVARCHAR(9) NOT NULL PRIMARY KEY,
	NgayLapPN DATE NOT NULL,
	MaNhanVien NVARCHAR(9) FOREIGN KEY REFERENCES NhanVien(MaNhanVien)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	TongTien INT,
)
GO
CREATE TABLE ChiTietHoaDon
(
	MaHoaDon NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES HoaDon(MaHoaDon)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	MaDichVu NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES DichVu(MaDichVu)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	DonGiaBan INT,
	SoLuongBan INT,
	PRIMARY KEY(MaHoaDon, MaDichVu)
)
GO
CREATE TABLE ChiTietPhieuNhap
(
	MaPhieuNhap NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES PhieuNhap(MaPhieuNhap)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	MaDichVu NVARCHAR(9) NOT NULL FOREIGN KEY REFERENCES DichVu(MaDichVu)
	ON UPDATE CASCADE
	ON DELETE CASCADE,
	DonGiaMua INT,
	SoLuongMua INT,
	PRIMARY KEY(MaPhieuNhap, MaDichVu)
)
GO


-- Thêm có bản ghi dữ liệu -- 
INSERT INTO LoaiDichVu (MaLoaiDV, TenLoaiDV)
VALUES
	(N'TA', N'Thức ăn'),
	(N'NN', N'Nước ngọt'),
	(N'BA', N'Bia'),
	(N'TC', N'Trái cây'),
	(N'OT', N'Khác');
GO
INSERT INTO DonViTinh (MaDonVi, TenDonVi)
VALUES
	(N'DV01', N'Chai'),
	(N'DV02', N'Thùng'),
	(N'DV03', N'Gói'),
	(N'DV04', N'Lon'),
	(N'DV05', N'Hộp'),
	(N'DV06', N'Kg'),
	(N'DV07', N'Lần');
GO
INSERT INTO NhaCungCap (MaNhaCungCap, TenNhaCungCap, SoDienThoai, DiaChi)
VALUES
    (N'NC01', N'Hòa Tiến', N'0123456789', N'Cam Lâm, Khánh Hòa'),
    (N'NC02', N'Thanh Bưởi', N'0987654321', N'Nha Trang, Khánh Hòa'),
    (N'NC03', N'Mỹ Linh', N'0365478962', N'Diên Khánh, Khánh Hòa'),
    (N'NC04', N'Văn Pháp', N'0456237891', N'Cam Lâm, Khánh Hòa'),
    (N'NC05', N'Thành Tín', N'0369854721', N'Diên Khánh, Khánh Hòa'),
    (N'NC06', N'Ngọc Minh', N'0974586213', N'Nha Trang, Khánh Hòa'),
    (N'NC07', N'Thu Vân', N'0912365478', N'Diên Khánh, Khánh Hòa'),
    (N'NC08', N'Huy Cận', N'0905123789', N'Cam Lâm, Khánh Hòa'),
    (N'NC09', N'Tố Hữu', N'0987654321', N'Nha Trang, Khánh Hòa'),
    (N'NC10', N'Thúy Vy', N'0365987412', N'Diên Khánh, Khánh Hòa');
GO
INSERT INTO LoaiPhong (MaLoaiPhong, TenLoaiPhong, GiaPhong) 
VALUES
	(N'LP01', N'Phòng Thường', 2000),
	(N'LP02', N'Phòng Cao Cấp', 5000),
	(N'LP03', N'Phòng Vip', 10000);
GO
INSERT INTO TrangThai (MaTrangThai, TenTrangThai)
VALUES
	(N'SS', N'Đang khả dụng'),
	(N'HD', N'Đang hoạt động'),
	(N'DD', N'Đang đặt'),
	(N'HT', N'Đã hoàn tất'),
	(N'NC', N'Đang nâng cấp'); 
GO
INSERT INTO DichVu (MaDichVu, TenDichVu, LoaiDichVu, DonViTinh, DonGia, SoLuongTon, NhaCungCap)
VALUES
    (N'SP01', N'Cơm cháy', N'TA', N'DV03', 10000, 800, N'NC01'),
    (N'SP02', N'Bia Heliken', N'BA', N'DV02', 150000, 1500, N'NC02'),
    (N'SP03', N'Cocacola', N'NN', N'DV04', 124000, 1200, N'NC03'),
    (N'SP04', N'Mít sấy', N'TA', N'DV03', 30000, 2500, N'NC04'),
    (N'SP05', N'Đào 45kg', N'OT', N'DV07', 1800000, 900, N'NC05'),
    (N'SP06', N'Bim bim', N'TA', N'DV01', 11000, 1700, N'NC06'),
    (N'SP07', N'Bia Sài Gòn', N'BA', N'DV02', 160000, 1000, N'NC07'),
    (N'SP08', N'Pepsi', N'NN', N'DV04', 220000, 500, N'NC08'),
    (N'SP09', N'Xoài cát', N'TC', N'DV06', 30000, 3000, N'NC09'),
    (N'SP10', N'Xoài non 50kg', N'OT', N'DV07', 1900000, 700, N'NC10'),
    (N'SP11', N'Lẩu thái', N'TA', N'DV05', 105000, 600, N'NC01'),
    (N'SP12', N'Bia Vina', N'BA', N'DV02', 155000, 1200, N'NC02'),
    (N'SP13', N'Mirinda', N'NN', N'DV02', 90000, 1800, N'NC03'),
    (N'SP14', N'Cóc chua', N'TC', N'DV06', 125000, 700, N'NC04'),
    (N'SP15', N'Spa thư giãn', N'OT', N'DV07', 1850000, 1300, N'NC05'),
    (N'SP16', N'Bít tết', N'TA', N'DV06', 115000, 2000, N'NC06'),
    (N'SP17', N'Rượu Soju', N'BA', N'DV03', 165000, 800, N'NC07'),
    (N'SP18', N'Sinh tố lúa mạch', N'NN', N'DV02', 22000, 1400, N'NC08'),
    (N'SP19', N'Dưa hấu', N'TC', N'DV06', 13000, 1000, N'NC09'),
    (N'SP20', N'Tay vịn', N'OT', N'DV07', 1950000, 1600, N'NC10'),
	(N'SP21', N'Trà sữa', N'TA', N'DV01', 40000, 500, N'NC01'),
    (N'SP22', N'Bia Tiger', N'BA', N'DV01', 140000, 800, N'NC02'),
    (N'SP23', N'Nước tăng lực', N'NN', N'DV04', 18000, 1200, N'NC03'),
    (N'SP24', N'Rong biển', N'TC', N'DV05', 90000, 2000, N'NC04'),
    (N'SP25', N'Cam sành', N'OT', N'DV06', 120000, 700, N'NC05'),
    (N'SP26', N'Mì xào hải sản', N'TA', N'DV05', 95000, 1500, N'NC06'),
    (N'SP27', N'Rượu Hennessy', N'BA', N'DV02', 320000, 600, N'NC07'),
    (N'SP28', N'Soda chanh', N'NN', N'DV01', 75000, 1000, N'NC08'),
    (N'SP29', N'Bánh tráng trộn', N'TC', N'DV03', 6000, 1800, N'NC09'),
    (N'SP30', N'Nho xanh', N'OT', N'DV06', 54000, 900, N'NC10');
GO
GO
INSERT INTO PhongHat (MaPhongHat, TenPhongHat, LoaiPhong, TrangThai)
VALUES
    (N'PH01', N'Thường 101', N'LP01', N'SS'),
    (N'PH02', N'Cao cấp 201', N'LP02', N'HD'),
    (N'PH03', N'Vip 301', N'LP03', N'SS'),
    (N'PH04', N'Thường 102', N'LP01', N'HD'),
    (N'PH05', N'Cao cấp 202', N'LP02', N'SS'),
    (N'PH06', N'Vip 302', N'LP03', N'HD'),
    (N'PH07', N'Thường 103', N'LP01', N'SS'),
    (N'PH08', N'Cao cấp 203', N'LP02', N'HD'),
    (N'PH09', N'Vip 303', N'LP03', N'SS'),
    (N'PH10', N'Thường 104', N'LP01', N'HD'),
	(N'PH11', N'Vip 102', N'LP01', N'SS'),
    (N'PH12', N'Cao cấp 103', N'LP02', N'HD'),
    (N'PH13', N'Vip 104', N'LP03', N'SS'),
    (N'PH14', N'Thường 109', N'LP01', N'HD'),
    (N'PH15', N'Cao cấp 203', N'LP02', N'SS'),
    (N'PH16', N'Vip 305', N'LP03', N'HD'),
    (N'PH17', N'Thường 107', N'LP01', N'SS'),
    (N'PH18', N'Cao cấp 205', N'LP02', N'HD'),
    (N'PH19', N'Vip 307', N'LP03', N'SS'),
    (N'PH20', N'Thường 206', N'LP01', N'HD'),
	(N'PH21', N'Thường 107', N'LP01', N'SS'),
    (N'PH22', N'Cao cấp 208', N'LP02', N'HD'),
    (N'PH23', N'Vip 309', N'LP03', N'SS'),
    (N'PH24', N'Thường 402', N'LP01', N'HD'),
    (N'PH25', N'Cao cấp 401', N'LP02', N'SS'),
    (N'PH26', N'Vip 404', N'LP03', N'HD'),
    (N'PH27', N'Thường 405', N'LP01', N'SS'),
    (N'PH28', N'Cao cấp 501', N'LP02', N'HD'),
    (N'PH29', N'Vip 502', N'LP03', N'SS'),
    (N'PH30', N'Thường 503', N'LP01', N'HD');
GO
INSERT INTO PhanQuyen (MaPhanQuyen, TenPhanQuyen)
VALUES
    (N'QL', N'Quản lý'),
    (N'NV', N'Nhân viên');
GO
INSERT INTO TaiKhoan (MaTaiKhoan, TenNguoiDung, TenDangNhap, MatKhau, PhanQuyen)
VALUES
    (N'TK01', N'Quản Lý', N'quanly', N'123456', N'QL'),
    (N'TK02', N'Thu Ngân', N'thungan', N'98765432', N'NV'),
    (N'TK03', N'Kiểm Kho', N'kiemkho', N'24681357', N'NV'),
    (N'TK04', N'Bảo Vệ', N'baove', N'13579246', N'NV');
GO
INSERT INTO KhachHang (MaKhachHang, TenKhachHang, DiaChi, SoDienThoai, MaTaiKhoan)
VALUES
	(N'KH01', N'Khách lẻ', NULL, NULL, NULL),
    (N'KH02', N'Nguyễn Đăng Khoa', N'Nha Trang', N'07892415621', NULL),
    (N'KH03', N'Nguyễn Dương Anh Tuấn', N'Khánh Hòa', N'0125126489', NULL),
    (N'KH04', N'Hồ Hoàng Kha', N'Ninh Thuận', N'0123453459', NULL),
    (N'KH05', N'Nguyễn Tuấn Kiệt', N'Khánh Hòa', N'0123475589', NULL),
    (N'KH06', N'Nguyễn Sanh Quốc Huy', N'Nha Trang', N'0123424683', NULL),
	(N'KH07', N'Nguyễn Thị Ngọc Trinh', N'Khánh Hòa', NULL, NULL),
    (N'KH08', N'Lê Kim Ngân', N'Hà Nội', N'07892415621', NULL),
    (N'KH09', N'Trần Mai Linh', N'Hồ Chí Minh', N'0125126489', NULL),
    (N'KH10', N'Trương Trường Giang', N'Đà Nẵng', N'0123453459', NULL),
    (N'KH11', N'Nguyễn Minh Tuấn', N'Hải Phòng', N'0123475589', NULL),
    (N'KH12', N'Nguyễn Hạnh Phương', N'Cần Thơ', N'0123424683', NULL),
	(N'KH13', N'Phạm Bích Thảo', N'Thanh Hóa', N'0123453459', NULL),
    (N'KH14', N'Vũ Linh Chi', N'Nghệ An', N'0123475589', NULL),
    (N'KH15', N'Hoàng Thanh Hương', N'Đồng Nai', N'0123424683', NULL);
	
GO
INSERT INTO DatPhong (MaDatPhong, GioVao, NgayDat, MaKhachHang, MaPhongHat, TrangThai)
VALUES
	(N'DP01', '12:00:00', '2022-06-25', N'KH01', N'PH01', N'HT'),
	(N'DP02', '14:30:00', '2022-07-30', N'KH02', N'PH02', N'HT'),
	(N'DP03', '13:15:00', '2022-10-10', N'KH03', N'PH01', N'HT'),
	(N'DP04', '10:00:00', '2023-02-15', N'KH04', N'PH03', N'HT'),
	(N'DP05', '19:45:00', '2023-03-20', N'KH05', N'PH02', N'HT'),
	(N'DP06', '16:30:00', '2023-04-25', N'KH01', N'PH01', N'HT'),
	(N'DP07', '18:00:00', '2023-05-30', N'KH03', N'PH03', N'HT'),
    (N'DP08', '11:30:00', '2023-06-10', N'KH02', N'PH02', N'HT'),
    (N'DP09', '13:45:00', '2023-07-15', N'KH04', N'PH01', N'HT'),
    (N'DP10', '15:00:00', '2023-08-20', N'KH05', N'PH03', N'HT'),
    (N'DP11', '17:15:00', '2023-09-25', N'KH01', N'PH02', N'HT'),
    (N'DP12', '19:30:00', '2023-10-30', N'KH03', N'PH01', N'HT'),
    (N'DP13', '21:45:00', '2023-11-05', N'KH02', N'PH03', N'HT'),
    (N'DP14', '13:00:00', '2023-12-10', N'KH06', N'PH01', N'HT'),
    (N'DP15', '15:15:00', '2023-12-25', N'KH05', N'PH02', N'HT');
GO
INSERT INTO NhanVien (MaNhanVien, TenNhanVien, NgaySinh, GioiTinh, Luong, MaTaiKhoan)
VALUES
    (N'NV01', N'Vũ Mai Linh', DATEFROMPARTS(1995, 1, 15), 0, 9000000, N'TK02'),
    (N'NV02', N'Lê Văn Pháp', DATEFROMPARTS(1998, 6, 20), 1, 12000000, N'TK03'),
    (N'NV03', N'Phan Anh Dũng', DATEFROMPARTS(2000, 9, 10), 1, 11000000, N'TK04');
GO
INSERT INTO HoaDon (MaHoaDon, NgayLapHD, MaNhanVien, MaDatPhong, TongTien, GioRa)
VALUES
    (N'HD01', '2022-06-25', N'NV01', N'DP01', 1500000, '15:00:00'),
    (N'HD02', '2022-07-30', N'NV01', N'DP02', 1200000, '17:30:00'),
    (N'HD03', '2022-10-10', N'NV02', N'DP03', 900000, '16:45:00'),
    (N'HD04', '2023-02-15', N'NV02', N'DP04', 2000000, '13:30:00'),
    (N'HD05', '2023-03-20', N'NV01', N'DP05', 800000, '22:15:00'),
    (N'HD06', '2023-04-25', N'NV02', N'DP06', 1700000, '19:00:00'),
    (N'HD07', '2023-05-30', N'NV01', N'DP07', 500000, '20:30:00'),
	(N'HD08', '2023-06-10', N'NV01', N'DP08', 1800000, '10:30:00'),
    (N'HD09', '2023-07-15', N'NV02', N'DP09', 2200000, '11:45:00'),
    (N'HD10', '2023-08-20', N'NV02', N'DP10', 1500000, '14:00:00'),
    (N'HD11', '2023-09-25', N'NV02', N'DP11', 1900000, '16:15:00'),
    (N'HD12', '2023-10-30', N'NV02', N'DP12', 800000, '18:30:00'),
    (N'HD13', '2023-11-05', N'NV01', N'DP13', 1300000, '20:45:00'),
    (N'HD14', '2023-12-10', N'NV01', N'DP14', 1000000, '12:00:00'),
    (N'HD15', '2023-12-25', N'NV02', N'DP15', 1700000, '14:15:00');
GO
INSERT INTO ChiTietHoaDon (MaHoaDon, MaDichVu, DonGiaBan, SoLuongBan)
VALUES
    (N'HD01', N'SP01', 50000, 10),
    (N'HD01', N'SP02', 80000, 5),
	(N'HD02', N'SP03', 70000, 3),
    (N'HD02', N'SP04', 60000, 4),
    (N'HD02', N'SP05', 100000, 2),
	(N'HD03', N'SP01', 50000, 8),
    (N'HD03', N'SP02', 80000, 6),
    (N'HD03', N'SP05', 100000, 1),
	(N'HD04', N'SP03', 70000, 2),
    (N'HD04', N'SP04', 60000, 5),
    (N'HD04', N'SP06', 90000, 3),
	(N'HD05', N'SP01', 50000, 7),
    (N'HD05', N'SP02', 80000, 4),
    (N'HD05', N'SP03', 70000, 2),
    (N'HD05', N'SP04', 60000, 3),
	(N'HD06', N'SP01', 50000, 10),
    (N'HD06', N'SP05', 100000, 5),
	(N'HD07', N'SP02', 80000, 3),
    (N'HD07', N'SP04', 60000, 4),
    (N'HD07', N'SP06', 90000, 2);
GO
INSERT INTO PhieuNhap (MaPhieuNhap, NgayLapPN, MaNhanVien, TongTien)
VALUES
    (N'PN01', '2022-06-15', N'NV02', 3000000),
    (N'PN02', '2022-09-03', N'NV02', 4500000),
    (N'PN03', '2023-01-22', N'NV02', 2000000),
    (N'PN04', '2023-04-08', N'NV02', 6000000),
    (N'PN05', '2023-05-10', N'NV02', 7500000);
GO
INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaDichVu, DonGiaMua, SoLuongMua)
VALUES
    (N'PN01', N'SP01', 150000, 100),
    (N'PN01', N'SP02', 200000, 80),
    (N'PN01', N'SP03', 120000, 120),
    (N'PN02', N'SP01', 150000, 50),
    (N'PN02', N'SP04', 180000, 70),
    (N'PN02', N'SP05', 250000, 60),
    (N'PN03', N'SP02', 200000, 100),
    (N'PN03', N'SP04', 180000, 150),
    (N'PN03', N'SP06', 160000, 80),
    (N'PN04', N'SP03', 120000, 90),
    (N'PN04', N'SP05', 250000, 120),
    (N'PN04', N'SP07', 220000, 70),
    (N'PN05', N'SP02', 200000, 150),
    (N'PN05', N'SP06', 160000, 80),
    (N'PN05', N'SP08', 190000, 100);

-- Các thủ tục -- 
GO
CREATE OR ALTER PROCEDURE ThongKeTonKho
    @NgayBatDau DATETIME = NULL,
    @NgayKetThuc DATETIME = NULL,
	@LoaiDichVu NVARCHAR(9)
AS
BEGIN
    SELECT
        dv.MaDichVu,
		dv.TenDichVu,
        dv.SoLuongTon - ISNULL(SUM(ctpn.SoLuongMua), 0) + ISNULL(SUM(cthd.SoLuongBan), 0) AS SoLuongTonDau,
        ISNULL(SUM(ctpn.SoLuongMua), 0) AS SoLuongMua,
        ISNULL(SUM(cthd.SoLuongBan), 0) AS SoLuongBan,
        dv.SoLuongTon AS SoLuongTonCuoi,
		ncc.TenNhaCungCap
    FROM
        DichVu dv
        LEFT JOIN ChiTietPhieuNhap ctpn ON dv.MaDichVu = ctpn.MaDichVu
        LEFT JOIN PhieuNhap pn ON ctpn.MaPhieuNhap = pn.MaPhieuNhap
        LEFT JOIN ChiTietHoaDon cthd ON dv.MaDichVu = cthd.MaDichVu
        LEFT JOIN HoaDon hd ON cthd.MaHoaDon = hd.MaHoaDon
        LEFT JOIN LoaiDichVu lm ON dv.LoaiDichVu = lm.MaLoaiDV
		JOIN NhaCungCap ncc ON dv.NhaCungCap = ncc.MaNhaCungCap
    WHERE
        (pn.NgayLapPN BETWEEN @NgayBatDau AND @NgayKetThuc OR hd.NgayLapHD BETWEEN @NgayBatDau AND @NgayKetThuc OR pn.NgayLapPN IS NULL OR hd.NgayLapHD IS NULL)
        AND (lm.MaLoaiDV = @LoaiDichVu OR @LoaiDichVu IS NULL)
    GROUP BY
        dv.MaDichVu, dv.SoLuongTon, ncc.TenNhaCungCap, dv.TenDichVu
END
GO
CREATE OR ALTER PROCEDURE TraCuuDichVu
(
	@TenDichVu NVARCHAR(100) = NULL,
	@LoaiDichVu NVARCHAR(9) = NULL,
	@NhaCungCap NVARCHAR(9) = NULL,
	@GiaMin VARCHAR(100) = NULL,
	@GiaMax VARCHAR(100) = NULL,
	@SoLuongMin VARCHAR(100) = NULL,
	@SoLuongMax VARCHAR(100) = NULL
) AS
BEGIN
	DECLARE @SqlStr NVARCHAR(4000)
	SELECT @SqlStr = 'SELECT * FROM DichVu WHERE (1=1) '
	IF @TenDichVu IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (TenDichVu LIKE N''%' + @TenDichVu + '%'')'
	IF @LoaiDichVu IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (LoaiDichVu LIKE ''%' + @LoaiDichVu + '%'')'
	IF @NhaCungCap IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (NhaCungCap LIKE ''%' + @NhaCungCap + '%'')'
	IF @GiaMin IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (DonGia >= Convert(int,''' + @GiaMin + '''))'
	IF @GiaMax IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (DonGia <= Convert(int,''' + @GiaMax + '''))'
	IF @SoLuongMin IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (SoLuongTon >= Convert(int,''' + @SoLuongMin + '''))'
	IF @SoLuongMax IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (SoLuongTon <= Convert(int,''' + @SoLuongMax + '''))'
	EXEC SP_EXECUTESQL @SqlStr
END
GO
CREATE OR ALTER PROCEDURE TraCuuKhachHang
(
	@TenKhachHang NVARCHAR(100) = NULL,
	@SoDienThoai NVARCHAR(100) = NULL,
	@DiaChi NVARCHAR(100) = NULL
) AS
BEGIN
	DECLARE @SqlStr NVARCHAR(4000)
	SELECT @SqlStr = 'SELECT * FROM KhachHang WHERE (1=1) '
	IF @TenKhachHang IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (TenKhachHang LIKE N''%' + @TenKhachHang + '%'')'
	IF @SoDienThoai IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (SoDienThoai LIKE N''%' + @SoDienThoai + '%'')'
	IF @DiaChi IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (DiaChi LIKE N''%' + @DiaChi + '%'')'
	EXEC SP_EXECUTESQL @SqlStr
END
GO
CREATE OR ALTER PROCEDURE TraCuuNhanVien
(
	@TenNhanVien NVARCHAR(100) = NULL,
	@ChucVu NVARCHAR(9) = NULL,
	@GioiTinh NVARCHAR(9) = NULL,
	@TuoiMin VARCHAR(100) = NULL,
	@TuoiMax VARCHAR(100) = NULL,
	@LuongMin VARCHAR(100) = NULL,
	@LuongMax VARCHAR(100) = NULL
) AS
BEGIN
	DECLARE @SqlStr NVARCHAR(4000)
	SELECT @SqlStr = 'SELECT * FROM NhanVien WHERE (1=1) '
	IF @TenNhanVien IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (TenNhanVien LIKE N''%' + @TenNhanVien + '%'')'
	IF @ChucVu IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (MaTaiKhoan LIKE ''%' + @ChucVu + '%'')'
	IF @GioiTinh IS NOT NULL
		SELECT @SqlStr = @SqlStr + 'AND (GioiTinh LIKE ''%' + @GioiTinh + '%'')'
	IF @TuoiMin IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (DATEDIFF(YEAR, NgaySinh, GETDATE()) >= Convert(int,''' + @TuoiMin + '''))'
	IF @TuoiMax IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (DATEDIFF(YEAR, NgaySinh, GETDATE()) <= Convert(int,''' + @TuoiMax + '''))'
	IF @LuongMin IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (Luong >= Convert(int,''' + @LuongMin + '''))'
	IF @LuongMax IS NOT NULL 
		SELECT @SqlStr = @SqlStr + 'AND (Luong <= Convert(int,''' + @LuongMax + '''))'
	EXEC SP_EXECUTESQL @SqlStr
END
EXEC dbo.TraCuuNhanVien N'', N'', N'', 0, 30, 0, 50000000