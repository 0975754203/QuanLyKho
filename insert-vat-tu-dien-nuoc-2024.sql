BEGIN TRANSACTION;

DECLARE @IdLoaiDonViTinh UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TuDienLoai WHERE MaLoai = N'LoaiDonViTinh');
IF @IdLoaiDonViTinh IS NULL
BEGIN
    THROW 50001, N'Chưa có loại từ điển LoaiDonViTinh trong bảng TuDienLoai.', 1;
END;

IF NOT EXISTS (SELECT 1 FROM NhomVatTu WHERE MaNhom = N'A')
    INSERT INTO NhomVatTu (Id, MaNhom, TenNhom) VALUES (NEWID(), N'A', N'Vật tư điện');

IF NOT EXISTS (SELECT 1 FROM NhomVatTu WHERE MaNhom = N'B')
    INSERT INTO NhomVatTu (Id, MaNhom, TenNhom) VALUES (NEWID(), N'B', N'Thiết bị vệ sinh, vật tư sửa chữa cấp thoát nước');

DECLARE @VatTu TABLE (
    MaNhom NVARCHAR(10),
    MaSanPham NVARCHAR(50),
    TenVatTu NVARCHAR(500),
    DonViTinh NVARCHAR(500),
    SoLuong DECIMAL(18, 2),
    KyHieuNhanMac NVARCHAR(500),
    XuatXu NVARCHAR(500),
    ThoiGianBaoHanh NVARCHAR(500),
    DonGia DECIMAL(18, 2)
);

INSERT INTO @VatTu (MaNhom, MaSanPham, TenVatTu, DonViTinh, SoLuong, KyHieuNhanMac, XuatXu, ThoiGianBaoHanh, DonGia)
VALUES

;WITH DonViTinhCanThem AS (
    SELECT DISTINCT DonViTinh FROM @VatTu WHERE ISNULL(DonViTinh, N'') <> N''
)
INSERT INTO TuDien (Id, IdLoaiTuDien, MaTuDien, TenTuDien, GhiChu)
SELECT NEWID(), @IdLoaiDonViTinh,
       CASE d.DonViTinh
           WHEN N'Chiếc' THEN N'CHIEC'
           WHEN N'Cuộn' THEN N'CUON'
           WHEN N'Máng' THEN N'MANG'
           WHEN N'Mét' THEN N'MET'
           WHEN N'Bộ' THEN N'BO'
           WHEN N'Tuýp' THEN N'TUYP'
           ELSE LEFT(N'DVT_' + CONVERT(NVARCHAR(36), NEWID()), 250)
       END,
       d.DonViTinh,
       N'Tự thêm khi import vật tư điện nước 2024'
FROM DonViTinhCanThem d
WHERE NOT EXISTS (
    SELECT 1
    FROM TuDien td
    WHERE td.IdLoaiTuDien = @IdLoaiDonViTinh
      AND td.TenTuDien = d.DonViTinh
);

INSERT INTO KhoSanPham (Id, MaSanPham, TenSanPham, GhiChu, IdDonViTinh, DonGia, IdNhomVatTu, XuatXu, KyhieuNhanmac, ThoiGianBaoHanh)
SELECT NEWID(),
       v.MaSanPham,
       v.TenVatTu,
       N'Số lượng theo hợp đồng: ' + CONVERT(NVARCHAR(50), v.SoLuong),
       td.Id,
       v.DonGia,
       nvt.Id,
       v.XuatXu,
       v.KyHieuNhanMac,
       v.ThoiGianBaoHanh
FROM @VatTu v
INNER JOIN NhomVatTu nvt ON nvt.MaNhom = v.MaNhom
INNER JOIN TuDien td ON td.IdLoaiTuDien = @IdLoaiDonViTinh AND td.TenTuDien = v.DonViTinh
WHERE NOT EXISTS (SELECT 1 FROM KhoSanPham sp WHERE sp.MaSanPham = v.MaSanPham);

SELECT N'Nhóm vật tư' AS NoiDung, COUNT(*) AS SoLuong FROM NhomVatTu WHERE MaNhom IN (N'A', N'B')
UNION ALL
SELECT N'Sản phẩm thêm mới/đã có theo mã HD2024', COUNT(*) FROM KhoSanPham WHERE MaSanPham LIKE N'HD2024-%';

COMMIT TRANSACTION;
-- Nếu muốn kiểm tra trước khi lưu thật, đổi dòng COMMIT TRANSACTION thành ROLLBACK TRANSACTION.
