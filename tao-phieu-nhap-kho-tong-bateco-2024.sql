BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @TenNhaCungCap NVARCHAR(500) = N'CÔNG TY CỔ PHẦN BATECO VIỆT NAM';
    DECLARE @SoHopDong NVARCHAR(500) = N'01/2024/HĐCCVT/BVK-BTC';
    DECLARE @NgayHopDong DATE = '2024-11-11';
    DECLARE @MaKho NVARCHAR(250) = N'KhoTong';
    DECLARE @IdKho UNIQUEIDENTIFIER;
    DECLARE @IdNhaCungCap UNIQUEIDENTIFIER;
    DECLARE @IdNguoiTao UNIQUEIDENTIFIER;
    DECLARE @IdGiaoDich UNIQUEIDENTIFIER = NEWID();
    DECLARE @MaGiaoDich NVARCHAR(250);
    DECLARE @Prefix NVARCHAR(50) = N'NHAP' + CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @SoThuTu INT;

    SELECT TOP 1 @IdKho = Id
    FROM Kho
    WHERE MaKho = @MaKho;

    IF @IdKho IS NULL
        THROW 50001, N'Không tìm thấy kho có MaKho = KhoTong.', 1;

    SELECT TOP 1 @IdNguoiTao = Id
    FROM TaiKhoan
    ORDER BY UserName;

    IF @IdNguoiTao IS NULL
        THROW 50002, N'Không tìm thấy tài khoản trong bảng TaiKhoan để gán IdNguoiTao.', 1;

    SELECT TOP 1 @IdNhaCungCap = idNhathaucc
    FROM NhaThauCungCap
    WHERE UPPER(LTRIM(RTRIM(TenNhaThau))) = UPPER(LTRIM(RTRIM(@TenNhaCungCap)));

    IF @IdNhaCungCap IS NULL
    BEGIN
        SET @IdNhaCungCap = NEWID();
        INSERT INTO NhaThauCungCap (idNhathaucc, TenNhaThau)
        VALUES (@IdNhaCungCap, @TenNhaCungCap);
    END;

    SELECT @SoThuTu = ISNULL(MAX(TRY_CONVERT(INT, SUBSTRING(MaGiaoDich, LEN(@Prefix) + 1, 6))), 0) + 1
    FROM KhoGiaoDich
    WHERE MaGiaoDich LIKE @Prefix + N'%';

    SET @MaGiaoDich = @Prefix + RIGHT(N'000000' + CONVERT(NVARCHAR(20), @SoThuTu), 6);

    DECLARE @VatTu TABLE (
        STT INT IDENTITY(1,1) PRIMARY KEY,
        TenVatTu NVARCHAR(500) NOT NULL,
        DonViTinh NVARCHAR(100) NOT NULL,
        SoLuong DECIMAL(18, 2) NOT NULL
    );

    INSERT INTO @VatTu (TenVatTu, DonViTinh, SoLuong)
    VALUES
    (N'Aptomat MCB 1P - 20A', N'Chiếc', 40),
    (N'Aptomat MCB 1P - 32A', N'Chiếc', 40),
    (N'Aptomat MCCB 3 pha - 50A', N'Chiếc', 10),
    (N'Aptomat 2P MCB - 40A', N'Chiếc', 20),
    (N'Máy khoan vít pin 21V', N'Chiếc', 2),
    (N'Máy cắt gạch cầm tay', N'Chiếc', 2),
    (N'Máy khoan điện cầm tay', N'Chiếc', 2),
    (N'Máy cắt sắt bàn 355mm', N'Chiếc', 2),
    (N'Băng dính điện', N'Cuộn', 120),
    (N'Bóng led tube 0.6m', N'Chiếc', 10),
    (N'Bóng đèn tròn Led buld 5W', N'Chiếc', 300),
    (N'Bóng đèn LED quả nhót', N'Chiếc', 50),
    (N'Đèn LED Panel 30x120/40W', N'Chiếc', 20),
    (N'Bóng đèn Panel 60x60/40W', N'Chiếc', 30),
    (N'Bóng đèn LED Tuýp T8 1.2m', N'Chiếc', 1500),
    (N'Bóng đèn LED Âm trần Downlight 110/ 9W', N'Chiếc', 60),
    (N'Bóng đèn LED Âm trần Downlight', N'Chiếc', 20),
    (N'Cây ghen nhựa', N'Máng', 250),
    (N'Dây điện 2x1.5', N'Mét', 1000),
    (N'Dây điện 2x2.5', N'Mét', 700),
    (N'Dây điện 2x4', N'Mét', 500),
    (N'Đế nổi', N'Chiếc', 100),
    (N'Đế âm', N'Chiếc', 200),
    (N'Đui đèn - đui đèn tròn - xoáy', N'Chiếc', 50),
    (N'Bình nóng lạnh 50 lít.', N'Chiếc', 10),
    (N'Thanh đốt bình nóng lạnh 50 lít', N'Chiếc', 100),
    (N'Dây chống giật bình nóng lạnh 50 lít', N'Chiếc', 100),
    (N'Rơ - le cọc bình nóng lạnh 50 lít', N'Chiếc', 100),
    (N'Gioăng cao su chống rò nước bình nóng lạnh 50 lít', N'Chiếc', 100),
    (N'Bình nóng lạnh 30 lít', N'Chiếc', 5),
    (N'Thanh đốt bình nóng lạnh 30 lít', N'Chiếc', 50),
    (N'Dây chống giật bình nóng lạnh 30 lít', N'Chiếc', 50),
    (N'Rơ - le bình nóng lạnh 30 lít', N'Chiếc', 50),
    (N'Gioăng cao su chống rò nước bình nóng lạnh 30 lít', N'Chiếc', 50),
    (N'Máy sưởi dầu (9 thanh sưởi)', N'Chiếc', 10),
    (N'Quạt sưởi sợi đốt', N'Chiếc', 10),
    (N'Hạt công tắc đảo chiều 10A', N'Chiếc', 50),
    (N'Hạt công tắc 20A', N'Chiếc', 80),
    (N'Hạt công tắc 10A', N'Chiếc', 100),
    (N'Hộp số quạt trần', N'Chiếc', 100),
    (N'Máng đèn Led tuýp đơn 1.2m - Không Ba Lát', N'Chiếc', 50),
    (N'Máng đèn Led tuýp đôi 1.2m - Không Ba Lát', N'Chiếc', 200),
    (N'Mặt ổ cắm đôi 2 Ổ cắm, 2 chấu, 2 công tắc', N'Chiếc', 200),
    (N'Mặt ổ cắm đôi 2 Ổ cắm 2 chấu', N'Chiếc', 200),
    (N'Ổ cắm  liền dây 3m, 3 ổ -10A', N'Chiếc', 50),
    (N'Ổ cắm  liền dây 5m, 6 ổ -10A', N'Chiếc', 60),
    (N'Quạt thông gió gắn tường 200', N'Chiếc', 200),
    (N'Quạt thông gió âm trần thẳng 300', N'Chiếc', 50),
    (N'Quạt trần 3 cánh sắt 1.4m', N'Chiếc', 70),
    (N'Quạt thông gió gắn tường 250 - 2 chiều', N'Chiếc', 50),
    (N'Quạt cây công nghiệp 750', N'Chiếc', 40),
    (N'Quạt cây sải cánh 45cm', N'Chiếc', 200),
    (N'Quạt treo tường sải cánh 45cm', N'Chiếc', 100),
    (N'Quạt công nghiệp treo tường 750', N'Chiếc', 20),
    (N'Quạt thông gió âm trần 25 x 25', N'Chiếc', 20),
    (N'Tụ quạt trần 3.5µ', N'Chiếc', 120),
    (N'Bộ bát sen tắm + dây sen', N'Bộ', 100),
    (N'Ống nhiệt Ø20', N'Mét', 50),
    (N'Ống nhiệt Ø25', N'Mét', 50),
    (N'Ống PVC Ø21', N'Mét', 50),
    (N'Ống PVC Ø27', N'Mét', 50),
    (N'Ống PVC Ø34', N'Mét', 50),
    (N'Ống PVC Ø110', N'Mét', 30),
    (N'Khóa van nhiệt Ø20', N'Chiếc', 30),
    (N'Khóa van nhiệt Ø25', N'Chiếc', 30),
    (N'Khoá van nhiệt Ø32', N'Chiếc', 30),
    (N'Khóa van nhiệt Ø40', N'Chiếc', 30),
    (N'Khóa van PVC Ø21', N'Chiếc', 20),
    (N'Khóa van PVC Ø27', N'Chiếc', 20),
    (N'Khóa van PVC Ø34', N'Chiếc', 20),
    (N'Đầu bịt ren trong Ø21', N'Chiếc', 100),
    (N'Đầu bịt ống nhiệt Ø20', N'Chiếc', 100),
    (N'Đầu bịt ống nhiệt Ø25', N'Chiếc', 100),
    (N'Đầu bịt ống nhiệt Ø32', N'Chiếc', 50),
    (N'Đầu bịt ống nhiệt Ø40', N'Chiếc', 50),
    (N'Vòi đồng Ø21 1/2', N'Chiếc', 50),
    (N'Cút nhiệt ren trong Ø20 1/2', N'Chiếc', 50),
    (N'Cút nhiệt ren ngoài Ø20 1/2', N'Chiếc', 50),
    (N'Cút nhiệt ren trong Ø25 1/2', N'Chiếc', 50),
    (N'Cút nhiệt ren ngoài Ø25 1/2', N'Chiếc', 50),
    (N'Cút góc nhiệt Ø20', N'Chiếc', 50),
    (N'Cút góc nhiệt Ø25', N'Chiếc', 50),
    (N'Mang xông nhiệt Ø20', N'Chiếc', 50),
    (N'Mang xông nhiệt Ø25', N'Chiếc', 50),
    (N'Kép Inox Ø21 1/2.', N'Chiếc', 50),
    (N'T - Inox Ø 20 1/2', N'Chiếc', 50),
    (N'T - Inox Ø 25 1/2', N'Chiếc', 50),
    (N'T - Inox Ø 20 1/2', N'Chiếc', 50),
    (N'T - Inox Ø 25 1/2', N'Chiếc', 50),
    (N'Van khoá PVC Ø21', N'Chiếc', 50),
    (N'Van khoá PVC Ø27', N'Chiếc', 50),
    (N'Van khoá PVC Ø34', N'Chiếc', 50),
    (N'Ống T - nhiệt Ø 20', N'Chiếc', 50),
    (N'Ống T - nhiệt Ø 25', N'Chiếc', 50),
    (N'Ống T - nhiệt Ø 32', N'Chiếc', 50),
    (N'Côn thu ống nhiệt Ø25 - Ø20', N'Chiếc', 50),
    (N'Côn thu ống nhiệt Ø32 - Ø20', N'Chiếc', 50),
    (N'Côn thu ống nhiệt Ø32 - Ø25', N'Chiếc', 50),
    (N'Van phao đồng DN 40', N'Chiếc', 8),
    (N'Van phao đồng DN 21', N'Chiếc', 10),
    (N'Van phao đồng DN 27', N'Chiếc', 10),
    (N'Cút PVC Ø21', N'Chiếc', 50),
    (N'Cút PVC Ø27', N'Chiếc', 50),
    (N'Cút PVC Ø110', N'Chiếc', 30),
    (N'Ống T  PVC Ø21', N'Chiếc', 50),
    (N'Ống T PVC Ø27', N'Chiếc', 50),
    (N'Ống T PVC Ø34', N'Chiếc', 50),
    (N'Ống T  PVC Ø110', N'Chiếc', 50),
    (N'Chếch PVC  Ø21', N'Chiếc', 50),
    (N'Chếch PVC  Ø27', N'Chiếc', 50),
    (N'Chếch PVC  Ø34', N'Chiếc', 50),
    (N'Chếch PVC  Ø110', N'Chiếc', 50),
    (N'Mang xông PVC  Ø21', N'Chiếc', 50),
    (N'Mang xông PVC  Ø27', N'Chiếc', 50),
    (N'Mang xông PVC  Ø34', N'Chiếc', 50),
    (N'Mang xông PVC  Ø110', N'Chiếc', 50),
    (N'Keo dính PVC 50g', N'Tuýp', 50),
    (N'Kéo cắt ống nhiệt', N'Chiếc', 5),
    (N'Cưa sắt cầm tay', N'Chiếc', 5),
    (N'Ga thoát sàn Inox D90', N'Chiếc', 200),
    (N'Ga thoát sàn Inox D76', N'Chiếc', 200),
    (N'Rọ chắn rác Inox Ø110', N'Chiếc', 50),
    (N'Rọ chắn rác Inox Ø48', N'Chiếc', 100),
    (N'Vòi nước lọc Inox RO dài 30 cm', N'Chiếc', 100),
    (N'Rắc co ren trong Ø20 1/2', N'Chiếc', 30),
    (N'Rắc co ren trong Ø25 1/2', N'Chiếc', 30),
    (N'Rắc co ren ngoài Ø20 1/2', N'Chiếc', 30),
    (N'Rắc co ren ngoài Ø25 1/2', N'Chiếc', 30),
    (N'Đầu bịt tròn uPVC Ø21', N'Chiếc', 100),
    (N'Đầu bịt tròn uPVC Ø27', N'Chiếc', 100),
    (N'Đầu bịt tròn uPVC Ø34', N'Chiếc', 100),
    (N'Đầu bịt tròn uPVC Ø48', N'Chiếc', 50),
    (N'Đầu bịt tròn uPVC Ø60', N'Chiếc', 50),
    (N'Đầu bịt tròn uPVC Ø76', N'Chiếc', 50),
    (N'Đầu bịt tròn uPVC Ø110', N'Chiếc', 20),
    (N'Băng tan', N'Cuộn', 150),
    (N'Bộ ruột xả nước bồn cầu - Loại tay gạt hông', N'Bộ', 100),
    (N'Bộ nắp bồn cầu', N'Bộ', 120),
    (N'Bồn cầu vệ sinh', N'Bộ', 20),
    (N'Chậu rửa mặt có chân đứng', N'Bộ', 20),
    (N'Dây cấp nước chậu rửa - loại ngắn', N'Chiếc', 300),
    (N'Vòi chậu rửa nóng lạnh - 1 chân', N'Chiếc', 50),
    (N'Vòi chậu rửa nóng lạnh - 2 chân', N'Chiếc', 100),
    (N'Vòi chậu rửa - 1 chân', N'Chiếc', 70),
    (N'Vòi chậu rửa dụng cụ - 1 chân', N'Chiếc', 40),
    (N'Vòi xịt vệ sinh', N'Chiếc', 200),
    (N'Vòi sen tắm nước nóng lạnh gắn tường', N'Chiếc', 20),
    (N'Van xả ấn bồn tiểu  loại ống thẳng', N'Bộ', 40),
    (N'Xi phông', N'Bộ', 400);

    IF OBJECT_ID('tempdb..#VatTuKhongKhop') IS NOT NULL
        DROP TABLE #VatTuKhongKhop;

    ;WITH KiemTraKhop AS (
        SELECT
            v.STT,
            v.TenVatTu,
            v.DonViTinh,
            v.SoLuong,
            COUNT(dvt.Id) AS SoDongKhop
        FROM @VatTu v
        LEFT JOIN KhoSanPham sp
            ON UPPER(LTRIM(RTRIM(sp.TenSanPham))) = UPPER(LTRIM(RTRIM(v.TenVatTu)))
        LEFT JOIN TuDien dvt
            ON dvt.Id = sp.IdDonViTinh
           AND UPPER(LTRIM(RTRIM(dvt.TenTuDien))) = UPPER(LTRIM(RTRIM(v.DonViTinh)))
        GROUP BY v.STT, v.TenVatTu, v.DonViTinh, v.SoLuong
    )
    SELECT STT, TenVatTu, DonViTinh, SoLuong, SoDongKhop
    INTO #VatTuKhongKhop
    FROM KiemTraKhop
    WHERE SoDongKhop <> 1;

    IF EXISTS (SELECT 1 FROM #VatTuKhongKhop)
    BEGIN
        SELECT *
        FROM #VatTuKhongKhop
        ORDER BY STT;

        THROW 50003, N'Có vật tư không khớp hoặc khớp nhiều hơn 1 sản phẩm theo Tên sản phẩm + Đơn vị tính. Xem result set #VatTuKhongKhop phía trên.', 1;
    END;

    INSERT INTO KhoGiaoDich (Id, MaGiaoDich, NgayTao, IdNguoiTao, LoaiGiaoDich, DaXoa, IdKhoaPhong, GhiChu, NguoiGiaoNhan, IdKho, IdGiaoDichCha, idNhaCungCap, SoHopDong, NgayHopDong)
    VALUES (@IdGiaoDich, @MaGiaoDich, GETDATE(), @IdNguoiTao, N'NHAP', 0, NULL,
            N'Nhập vật tư theo hợp đồng ' + @SoHopDong, NULL, @IdKho, NULL, @IdNhaCungCap, @SoHopDong, @NgayHopDong);

    ;WITH MatchSanPham AS (
        SELECT
            v.STT,
            v.SoLuong,
            sp.Id AS IdSanPham,
            sp.DonGia
        FROM @VatTu v
        INNER JOIN KhoSanPham sp
            ON UPPER(LTRIM(RTRIM(sp.TenSanPham))) = UPPER(LTRIM(RTRIM(v.TenVatTu)))
        INNER JOIN TuDien dvt
            ON dvt.Id = sp.IdDonViTinh
           AND UPPER(LTRIM(RTRIM(dvt.TenTuDien))) = UPPER(LTRIM(RTRIM(v.DonViTinh)))
    ),
    ChiTietGom AS (
        SELECT IdSanPham, SUM(SoLuong) AS SoLuong, MAX(DonGia) AS DonGia
        FROM MatchSanPham
        GROUP BY IdSanPham
    )
    INSERT INTO KhoGiaoDichChiTiet (Id, IdGiaoDich, IdSanPham, SoLuong, DonGia)
    SELECT NEWID(), @IdGiaoDich, IdSanPham, SoLuong, ISNULL(DonGia, 0)
    FROM ChiTietGom;

    ;WITH MatchSanPham AS (
        SELECT
            v.SoLuong,
            sp.Id AS IdSanPham
        FROM @VatTu v
        INNER JOIN KhoSanPham sp
            ON UPPER(LTRIM(RTRIM(sp.TenSanPham))) = UPPER(LTRIM(RTRIM(v.TenVatTu)))
        INNER JOIN TuDien dvt
            ON dvt.Id = sp.IdDonViTinh
           AND UPPER(LTRIM(RTRIM(dvt.TenTuDien))) = UPPER(LTRIM(RTRIM(v.DonViTinh)))
    ),
    TonGom AS (
        SELECT IdSanPham, SUM(SoLuong) AS SoLuong
        FROM MatchSanPham
        GROUP BY IdSanPham
    )
    MERGE KhoTon AS target
    USING TonGom AS source
        ON target.IdSanPham = source.IdSanPham
       AND target.IdKho = @IdKho
    WHEN MATCHED THEN
        UPDATE SET target.SoLuong = target.SoLuong + source.SoLuong
    WHEN NOT MATCHED THEN
        INSERT (Id, IdSanPham, SoLuong, IdKho)
        VALUES (NEWID(), source.IdSanPham, source.SoLuong, @IdKho);

    SELECT
        @MaGiaoDich AS MaGiaoDich,
        @IdGiaoDich AS IdGiaoDich,
        @TenNhaCungCap AS TenNhaCungCap,
        @SoHopDong AS SoHopDong,
        @NgayHopDong AS NgayHopDong,
        COUNT(*) AS SoDongChiTiet,
        SUM(SoLuong) AS TongSoLuong
    FROM KhoGiaoDichChiTiet
    WHERE IdGiaoDich = @IdGiaoDich;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
