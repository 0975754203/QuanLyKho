
using BusinessLogic.Utils;
//using DocumentFormat.OpenXml.Spreadsheet;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Spire.Pdf;
using Spire.Xls;
using Aspose.Cells;
using WorkbookAspose = Aspose.Cells.Workbook;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace BusinessLogic
{
    public static class BusinessUtils
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        /// <summary>
        /// [0] User Id
        /// [1] User Name
        /// [2] Id DonVi
        /// [3] iQuyen
        /// [4] Tên đơn vị
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string[] GetUserInfo(HttpCookie cookie)
        {
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }
            return cookie.Value.SplitEmbeddedLength();
        }
        public static string GetCookieBoDuLieu(HttpCookie cookie)
        {
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }
            return cookie.Value.Substring(0, cookie.Value.Length - 1);
        }
        public static string GetCookieKyThi(HttpCookie cookie)
        {
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }
            return cookie.Value;
        }
        public static string GetCookie(HttpCookie cookie)
        {
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return null;
            }
            return cookie.Value;
        }
   
        public static string DoiText(string values)
        {
            string temp = "aáàạảãâấầậẩẫăắằặẳẵeéèẹẻẽêếềệểễoóòọỏõôốồộổỗơớờợởỡuúùụủũưứừựửữiíìịỉĩdđyýỳỵỷỹ";
            string temp1 = "a00a01a02a03a04a05a06a07a08a09a10a11a12a13a14a15a16a17e00e01e02e03e04e05e06e07e08e09e10e11o00o01o02o03o04o05o06o07o08o09o10o11o12o13o14o15o16o17u00u01u02u03u04u05u06u07u08u09u10u11i00i01i02i03i04i05d00d01y00y01y02y03y04y05";
            for (int i = 0; i < temp.Length; i++)
            {
                values = values.Replace(temp.Substring(i, 1), temp1.Substring(i * 3, 3));
            }
            return values;
        }
        public static string GetDoiTuongUuTien(string value)
        {
            var strUuTien = "";
            if (value.IsNullOrEmpty())
            {
                return strUuTien;
            }

            if (value.IndexOf("1") > -1)
            {
                strUuTien = strUuTien + "Con liệt sỹ, ";
            }
            if (value.IndexOf("2") > -1)
            {
                strUuTien = strUuTien + "Con thương binh, bệnh binh hoặc hưởng như thương binh trên 81%, ";
            }
            if (value.IndexOf("3") > -1)
            {
                strUuTien = strUuTien + "Con của người nhiễm chất độc hóa học, ";
            }
            if (value.IndexOf("4") > -1)
            {
                strUuTien = strUuTien + "Con của người hoạt động cách mạnh đến khởi nghĩa tháng tám năm 1945, ";
            }
            if (value.IndexOf("5") > -1)
            {
                strUuTien = strUuTien + "Con Anh hùng lao động, con Anh hùng LLVT, con bà mẹ Việt Nam anh hùng, ";
            }
            if (value.IndexOf("6") > -1)
            {
                strUuTien = strUuTien + "Con thương binh, bệnh binh hoặc hưởng như thương binh dưới 81%, ";
            }
            if (value.IndexOf("7") > -1)
            {
                strUuTien = strUuTien + "Học sinh là người dân tộc thiểu số hoặc có cha mẹ là người dân tộc thiểu số, ";
            }
            if (value.IndexOf("8") > -1)
            {
                strUuTien = strUuTien + "Học sinh đang sinh sống trong vùng có điều kiện KT - XH khó khăn, ";
            }
            if (value.IndexOf("9") > -1)
            {
                strUuTien = strUuTien + "Học sinh tàn tật, khuyết tật, kém phát triển, nhiễm CĐHH, mồ côi, diện hộ đói nghèo, ";
            }

            return strUuTien;
        }
        public static string GetDoiTuongKhuyenKhich(string loai, string giai)
        {
            var strKhuyenKhich = "";
            if (loai.IsNullOrEmpty() || giai.IsNullOrEmpty())
            {
                return strKhuyenKhich;
            }
            if (loai == "1")
            {
                strKhuyenKhich = "Cuộc thi khoa học kỹ thuật cấp tỉnh: ";
            }
            else if (loai == "2")
            {
                strKhuyenKhich = "Hội thi Giai điệu tuổi hồng cấp tỉnh: ";
            }
            else if (loai == "3")
            {
                strKhuyenKhich = "Hội khỏe Phù Đổng cấp tỉnh: ";
            }
            if (giai == "1")
            {
                strKhuyenKhich += " Giải Nhất.";
            }
            else if (giai == "2")
            {
                strKhuyenKhich += " Giải Nhì.";
            }
            else if (giai == "3")
            {
                strKhuyenKhich += " Giải Ba.";
            }
            else if (giai == "4")
            {
                strKhuyenKhich += " Khuyến khích.";
            }
            else if (giai == "V")
            {
                strKhuyenKhich += " Huy chương vàng.";
            }
            else if (giai == "B")
            {
                strKhuyenKhich += " Huy chương bạc.";
            }
            else if (giai == "Đ")
            {
                strKhuyenKhich += " Huy chương đồng.";
            }
            else if (giai == "K")
            {
                strKhuyenKhich += " Bằng khen.";
            }




            return strKhuyenKhich;
        }
        public static string GetLoaiTotNghiep(string value)
        {
            string sloai = "";
            if (value == cfLoaiTN.D)
            {
                sloai = cfTenLoaiTN.D;
            }
            else if (value == cfLoaiTN.K)
            {
                sloai = cfTenLoaiTN.K;
            }
            return sloai;
        }
        public static string GetLoaiTotNghiep1(string value)
        {
            string sloai = "";
            if (value == cfLoaiTN.D)
            {
                sloai = cfTenLoaiTN.D;
            }
            else if (value == cfLoaiTN.K)
            {
                sloai = cfTenLoaiTN.K;
            }
            return sloai;
        }
        public static string GetHSGLop9CapTinh(string value)
        {
            string sloai = "";
            if (value == "1")
            {
                sloai = cfGiaiHSGTinh.Giai1;
            }
            else if (value == "2")
            {
                sloai = cfGiaiHSGTinh.Giai2;
            }
            else if (value == "3")
            {
                sloai = cfGiaiHSGTinh.Giai3;
            }
            else if (value == "4")
            {
                sloai = cfGiaiHSGTinh.Giai4;
            }
            return sloai;
        }

        public static string GetHSGLop9Mon(string value)
        {
            string sloai = "";
            if (value == "L")
            {
                sloai = "KHTN (Chủ đề Năng lượng và sự biến đổi)";
            }
            else if (value == "H")
            {
                sloai = "KHTN (Chủ đề Chất và sự biến đổi của chất)";
            }
            else if (value == "S")
            {
                sloai = "KHTN (Chủ đề Vật sống)";
            }
            else if (value == "U")
            {
                sloai = "Lịch sử và Địa lí (Phân môn Lịch sử)";
            }
            else if (value == "D")
            {
                sloai = "Lịch sử và Địa lí (Phân môn Địa lí)";
            }
            else if (value == "T")
            {
                sloai = "Toán";
            }
            else if (value == "V")
            {
                sloai = "Ngữ văn";
            }
            else if (value == "A")
            {
                sloai = "Tiếng Anh";
            }
            else if (value == "N")
            {
                sloai = "Tiếng Nga";
            }
            else if (value == "P")
            {
                sloai = "Tiếng Pháp";
            }
            else if (value == "I")
            {
                sloai = "Tin";
            }
            return sloai;
        }
        public static string GetGiaiKH_KT(string value)
        {
            string sloai = cfGiaiKH_KT.Giai0;
            if (value == "1")
            {
                sloai = cfGiaiKH_KT.Giai1;
            }
            else if (value == "2")
            {
                sloai = cfGiaiKH_KT.Giai2;
            }
            else if (value == "3")
            {
                sloai = cfGiaiKH_KT.Giai3;
            }
            else if (value == "4")
            {
                sloai = cfGiaiKH_KT.Giai4;
            }
            return sloai;
        }
        public static string GetGiaiVanHoa(string value)
        {
            string sloai = "";
            if (value == "1")
            {
                sloai = cfGiaiVanHoa.Giai1;
            }
            else if (value == "2")
            {
                sloai = cfGiaiVanHoa.Giai2;
            }
            else if (value == "3")
            {
                sloai = cfGiaiVanHoa.Giai3;
            }
            else if (value == "4")
            {
                sloai = cfGiaiVanHoa.Giai4;
            }
            return sloai;
        }
        public static string GetChungChiTAnhQT(string value)
        {
            string sloai = cfChungChiTiengAnh.ChungChi0;
            if (value == "C1")
            {
                sloai = cfChungChiTiengAnh.ChungChi2;
            }
            else if (value == "C2")
            {
                sloai = cfChungChiTiengAnh.ChungChi1;
            }
            else if (value == "B1")
            {
                sloai = cfChungChiTiengAnh.ChungChi4;
            }
            else if (value == "B2")
            {
                sloai = cfChungChiTiengAnh.ChungChi3;
            }
            else if (value == "A1")
            {
                sloai = cfChungChiTiengAnh.ChungChi5;
            }
            else if (value == "A2")
            {
                sloai = cfChungChiTiengAnh.ChungChi6;
            }
            else if (value == "A3")
            {
                sloai = cfChungChiTiengAnh.ChungChi7;
            }
            else if (value == "A4")
            {
                sloai = cfChungChiTiengAnh.ChungChi8;
            }
            else if (value == "A5")
            {
                sloai = cfChungChiTiengAnh.ChungChi9;
            }
            else if (value == "A6")
            {
                sloai = cfChungChiTiengAnh.ChungChi10;
            }
            return sloai;
        }
        public static string GetLoaiHanhKiem(string value)
        {
            string sloai = "";
            if (value == cfLoaiHanhKiem.T)
            {
                sloai = cfTenLoaiHanhKiem.T;
            }
            else if (value == cfLoaiHanhKiem.K)
            {
                sloai = cfTenLoaiHanhKiem.K;
            }
            else if (value == cfLoaiHanhKiem.D)
            {
                sloai = cfTenLoaiHanhKiem.D;
            }
            else if (value == cfLoaiHanhKiem.C)
            {
                sloai = cfTenLoaiHanhKiem.C;
            }
            else if (value == cfLoaiHanhKiem.X)
            {
                sloai = cfTenLoaiHanhKiem.X;
            }
            return sloai;
        }
        public static string GetLoaiHocLuc(string value)
        {
            string sloai = "";
            if (value == cfLoaiHocLuc.T)
            {
                sloai = cfTenLoaiHocLuc.T;
            }
            else if (value == cfLoaiHocLuc.K)
            {
                sloai = cfTenLoaiHocLuc.K;
            }
            else if (value == cfLoaiHocLuc.D)
            {
                sloai = cfTenLoaiHocLuc.D;
            }
            else if (value == cfLoaiHocLuc.C)
            {
                sloai = cfTenLoaiHocLuc.C;
            }
            else if (value == cfLoaiHocLuc.X)
            {
                sloai = cfTenLoaiHocLuc.X;
            }
            return sloai;
        }
        public static string NumberToText(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            if (sNumber.IsNullOrEmpty())
            {
                return "Không" + (suffix ? "" : "");
            }
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? "" : "");
        }
        //public static string ExcelToPdf(string inPatch, string outPatch, PaperSizeType paperSizeType, PageOrientationType pageOrientationType)
        //{
        //    //var fullPathIn = HostingEnvironment.MapPath("~/Content/Report/Templates/BaoCaoKetQuaXetTNAll.xlsx");
        //    //var fullPathOut = HostingEnvironment.MapPath("~/Content/UploadedPDF/Templates/BaoCaoKetQuaXetTNAll.pdf");
        //    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(inPatch);
        //    Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
        //    worksheet.PageSetup.PaperSize = paperSizeType;
        //    worksheet.PageSetup.Orientation = pageOrientationType;
        //    // Save the document in PDF format
        //    workbook.Save(outPatch, SaveFormat.Pdf);
        //    return outPatch;
        //}
        public static string ExcelToPdf(string inPatch, string outPatch)
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(inPatch);
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                workbook.Worksheets[i].PageSetup.IsFitToPage = true;
            }
            workbook.SaveToFile(outPatch, Spire.Xls.FileFormat.PDF);
            return "";

        }
        public static List<string> TachTenDonVi(string sTen)
        {
            var leng = sTen.Length;
            var tach = leng / 6;
            if (tach == 0)
            {
                tach = 1;
            }
            List<string> result = new List<string>();
            var item1 = sTen.Substring(0, tach);
            var item2 = sTen.Substring(tach, leng - tach * 2);
            var item3 = sTen.Substring(leng - tach);
            result.Add(item1);
            result.Add(item2);
            result.Add(item3);
            return result;
        }
        public static string TachTenTruong(string sTen)
        {
            if (sTen.ToLower().Contains("trường"))
            {
                sTen = sTen.Substring(7);
                if (sTen.ToLower().Contains("thcs"))
                {
                    return sTen.Substring(5);
                }
                return sTen;
            }
            return sTen;
        }
        public static string TachTenNoiSinh(string sTen)
        {
            if (sTen.ToLower().Contains("tỉnh"))
            {
                return sTen.Substring(5);
            }
            if (sTen.ToLower().Contains("tp"))
            {
                return sTen.Substring(3);
            }
            return sTen;
        }
        public static string TachTenHuyen(string sTen)
        {
            if (sTen.ToLower().Contains("huyện"))
            {
                return sTen.Substring(5);
            }
            if (sTen.ToLower().Contains("tp"))
            {
                return sTen.Substring(2);
            }
            if (sTen.ToLower().Contains("thành phố"))
            {
                return sTen.Substring(9).Trim();
            }
            return sTen;
        }
        public static DataTable GetExcelWorksheetData(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            Aspose.Cells.Workbook workBook = new Aspose.Cells.Workbook(fileName);
            Aspose.Cells.Worksheet workSheet = workBook.Worksheets[0]; // Assumption is that we are working with one worksheet
            DataTable dataTable = new DataTable();
            Aspose.Cells.ExportTableOptions options = new Aspose.Cells.ExportTableOptions();
            options.ExportColumnName = true;
            options.IsVertical = true;
            options.ExportAsString = true;
            dataTable = workSheet.Cells.ExportDataTable(0, 0, workSheet.Cells.MaxDataRow + 1, workSheet.Cells.MaxColumn + 1, options);
            return dataTable;
        }

        public static string DocSo(string str)
        {
            string[] word = { "", " một", " hai", " ba", " bốn", " năm", " sáu", " bẩy", " tám", " chín" };
            string[] million = { "", " mươi", " trăm", "" };
            string[] billion = { "", "", "", " nghìn", "", "", " triệu", "", "" };
            string result = "{0}";
            int count = 0;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (count > 0 && count % 9 == 0)
                    result = string.Format(result, "{0} tỷ");
                if (!(count < str.Length - 3 && count > 2 && str[i].Equals('0') && str[i - 1].Equals('0') && str[i - 2].Equals('0')))
                    result = string.Format(result, "{0}" + billion[count % 9]);
                if (!str[i].Equals('0'))
                    result = string.Format(result, "{0}" + million[count % 3]);
                else if (count % 3 == 1 && count > 1 && !str[i - 1].Equals('0') && !str[i + 1].Equals('0'))
                    result = string.Format(result, "{0} lẻ");
                var num = Convert.ToInt16(str[i].ToString());
                result = string.Format(result, "{0}" + word[num]);
                count++;
            }
            result = result.Replace("{0}", "");
            return result.Trim();
        }




    }


}