using BusinessLogic.Model;
using Grpc.Core;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BusinessLogic.Management
{
    public class ReportExcel
    {
        private string m_strTemplateFileNameWithPath;
        private string m_strOutputFileNameWithPath;
        private string m_strOutputFileName;
        private ExcelPackage pck;
        private ExcelWorksheet m_objExcelWorksheet;

        private bool m_init_successful;
        public Hashtable FindAndReplaceCollection;
        /// <summary>
        /// Hàm khởi tạo chuyền vào tên file template và tên file report
        /// </summary>
        /// <param name="i_strTemplatesFileName"></param>
        /// <param name="i_strFileName"></param>
        public ReportExcel(string i_strTemplatesFileName, string i_strOutPath = "", string OutFileName = "")
        {
            //Kiểm tra folder tồn tại không
            if (i_strOutPath != "")
            {
                if (!Directory.Exists(i_strOutPath))
                {
                    Directory.CreateDirectory(i_strOutPath);
                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fullPath = HostingEnvironment.MapPath("~/Content/Report");
            var fullPathOut = i_strOutPath;
            if (i_strOutPath.IsNotNullOrEmpty())
            {
                m_strOutputFileName = fullPathOut + OutFileName;
            }
            else
            {
                m_strOutputFileName = fullPath + @"\Output\" + i_strTemplatesFileName;
            }
            m_strTemplateFileNameWithPath = fullPath + @"\Templates\" + i_strTemplatesFileName;
            // Nếu có template trong folder domain thì ưu tiên lấy trong đó

            m_strOutputFileNameWithPath = m_strOutputFileName;
            m_init_successful = false;
            FindAndReplaceCollection = new Hashtable();
            if (File.Exists(m_strOutputFileNameWithPath))
            {
                File.Delete(m_strOutputFileNameWithPath);
            }
            pck = new ExcelPackage(new FileInfo(m_strOutputFileNameWithPath), new FileInfo(m_strTemplateFileNameWithPath));
            m_objExcelWorksheet = pck.Workbook.Worksheets.FirstOrDefault();
            m_init_successful = true;
        }
        public void Dispose()
        {
            pck.Dispose();
        }
        public ReportExcel AddFindAndReplaceItem(object i_obj_find, object i_obj_replace)
        {
            FindAndReplaceCollection.Add(i_obj_find, i_obj_replace);
            return this;
        }
        public ReportExcel ClearFindAndReplaceItem(object i_obj_find, object i_obj_replace)
        {
            FindAndReplaceCollection.Clear();
            return this;
        }
        public ReportExcel FindAndReplace()
        {
            try
            {
                if (!m_init_successful) return this;
                foreach (var v_str_find in FindAndReplaceCollection.Keys)
                {
                    var v_str_replace = FindAndReplaceCollection[v_str_find].ToString();
                    var cell = m_objExcelWorksheet.Cells.Where(x => x.Text.Contains(v_str_find.ToString())).FirstOrDefault();
                    if (cell != null)
                    {
                        cell.Value = cell.Text.Replace(v_str_find.ToString(), v_str_replace);
                    }
                }
                return this;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }
        public ReportExcel goToSheet(int sheetIndex)
        {
            if (pck.Workbook.Worksheets.Count >= sheetIndex)
                m_objExcelWorksheet = pck.Workbook.Worksheets[sheetIndex];
            return this;
        }
        /// <summary>
        /// Nhảy tới sheet theo tên
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public ReportExcel goToSheet(string sheetName)
        {
            m_objExcelWorksheet = pck.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName);
            return this;
        }
        public ReportExcel FindAndReplaceAll()
        {
            try
            {
                if (!m_init_successful) return this;
                foreach (var v_str_find in FindAndReplaceCollection.Keys)
                {
                    var lstReplace = (IList)FindAndReplaceCollection[v_str_find];
                    var cell = m_objExcelWorksheet.Cells.Where(x => x.Text.Contains(v_str_find.ToString())).ToList();
                    if (cell.Count() > 0)
                    {
                        for (int i = 0; i < lstReplace.Count; i++)
                        {
                            cell[i].Value = cell[i].Text.Replace(v_str_find.ToString(), lstReplace[i].ToString());
                        }
                    }
                    for (int j = lstReplace.Count; j < cell.Count(); j++)
                    {
                        cell[j].Value = cell[j].Text.Replace(v_str_find.ToString(), "");
                    }
                }
                return this;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }
        /// <summary>
        /// Sửa và bổ sung headers
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="ordinalRow">Hiển thị số cột</param>
        /// <param name="mergeCount">Số hàng của headers được merge lại với nhau</param>
        /// <returns></returns>
        public ReportExcel UpdateHeader(IList headers, int colIndex, int rowIndex, bool ordinalRow = false, int mergeCount = 0, bool isAddColumn = false)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                if (isAddColumn)
                {
                    m_objExcelWorksheet.InsertColumn(colIndex + i, 1, colIndex);
                }

                var cell = m_objExcelWorksheet.Cells[rowIndex, colIndex + i];
                if (mergeCount > 0)
                {
                    cell = m_objExcelWorksheet.Cells[rowIndex, colIndex + i, rowIndex + mergeCount, colIndex + i];
                    cell.Merge = true;
                }
                if (ordinalRow)
                {
                    m_objExcelWorksheet.Cells[rowIndex + 1, colIndex + i, rowIndex + mergeCount + 1, colIndex + i].Value = string.Format("({0})", colIndex + i);
                }
                cell.Style.Font.Bold = true;
                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell.Value = headers[i];
            }
            return this;
        }
        /// <summary>
        /// Sửa và bổ sung headers
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="start"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public ReportExcel UpdateHeader(IList headers, int row)
        {
            return UpdateHeader(headers, 0, row);
        }

        /// <summary>
        /// Ghi dữ liệu vào file Excel
        /// -> Hình dung dữ liệu trong tập DataTable có hình vuông và bê đặt vào file Excel
        /// -> Xác định tọa độ cột, hàng tương ứng giữa chúng :)
        /// </summary>
        /// <param name="i_DataTabel">Tập chứa dữ liệu báo cáo</param>
        /// <param name="i_intStartRow">Hàng (row) bắt đầu ghi dữ liệu trong file Excel</param>
        /// <param name="i_intStartCol">Cột (column) bắt đầu ghi dữ liệu trong file Excel</param>
        /// <param name="i_endColData">Cột dữ liệu kết thúc trong DataTable</param>
        /// <param name="is_newRow">Chèn thêm hàng và copy style -> số hàng được chèn thêm bằng với số hàng dữ liệu</param>
        /// <param name="i_startColData">Cột bắt đầu lấy dữ liệu trong DataTable</param>
        /// <param name="iSoRowBo">Số row trong DataTable bỏ. Sẽ bỏ từ cuối lên</param>
        /// <returns></returns>
        public MemoryStream Export2Excel(DataTable i_DataTabel, int i_intStartRow, int i_intStartCol, int i_endColData, bool is_newRow = true,
            int i_startColData = 0, int iSoRowBo = 0)
        {
            try
            {
                var result = new MemoryStream();
                if (!m_init_successful) return result;
                int i_iExcelRow = i_intStartRow;
                int i_iExcelCol = i_intStartCol;
                if (iSoRowBo != 0)
                {
                    for (int i = 1; i <= iSoRowBo; i++)
                    {
                        var countTable = i_DataTabel.Rows.Count;
                        DataRow dr = i_DataTabel.Rows[countTable - i];
                        dr.Delete();
                    }
                    i_DataTabel.AcceptChanges();
                }
                if (i_DataTabel.Rows.Count > 1)
                {
                    if (is_newRow)
                    {
                        m_objExcelWorksheet.InsertRow(i_intStartRow + 1, i_DataTabel.Rows.Count - 1, i_intStartRow);
                    }
                }

                foreach (DataRow item in i_DataTabel.Rows)
                {
                    int dem = 0;
                    for (int i = i_startColData; i < i_endColData; i++)
                    {
                        if (item[i] != null && item[i].ToString() != null)
                        {
                            var cell = m_objExcelWorksheet.Cells[i_iExcelRow, i_iExcelCol + dem];
                            var value = item[i];
                            if (value.ToString().IndexOf("<b>") == 0)
                            {
                                cell.Style.Font.Bold = true;
                                value = value.ToString().Substring(3);
                            }
                            cell.Value = value;
                            dem++;
                        }
                    }
                    i_iExcelRow++;
                }
                result = new MemoryStream(pck.GetAsByteArray()); //Get updated stream
                return result;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }
        public string ExportExcelByList(IEnumerable<object> data, int i_intStartRow, int i_intStartCol, string[] lstColumn)
        {
            try
            {
                var result = new MemoryStream();
                if (!m_init_successful) return "";
                if (data != null && data.Count() > 0)
                {
                    if (lstColumn == null)
                        lstColumn = data.FirstOrDefault().GetType().GetProperties().Select(x => x.Name).ToArray<string>();

                    m_objExcelWorksheet.InsertRow(i_intStartRow + 1, data.Count(), i_intStartRow);
                    int rowIndex = 1;
                    int stt = 1;
                    var height = m_objExcelWorksheet.Cells[i_intStartRow, i_intStartCol].EntireRow.Height;
                    foreach (var itemdata in data)
                    {
                        Type type = itemdata.GetType();
                        m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Value = stt;
                        // ThaySTT để thay vào cột STT vào reset lại số TT = 1
                        if (type.GetProperty("ThaySTT") != null)
                        {
                            PropertyInfo property1 = type.GetProperty("ThaySTT");
                            object obj1 = property1.GetValue(itemdata, null);
                            if (obj1 != null && obj1.ToString() != "")
                            {
                                m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Value = obj1;
                                m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Style.Font.Bold = true;
                                stt = 0;
                            }
                        }
                        int iCol = 1;
                        foreach (string item in lstColumn)
                        {
                            if (item == "")
                            {
                                var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                cell.Value = "";
                                cell.EntireRow.Height = height;
                            }
                            else
                            {
                                var arr = Regex.Split(item, ",");

                                if (arr.Length > 1)
                                {
                                    #region Truong hop truyen vao kieu so sanh
                                    PropertyInfo check = type.GetProperty(arr[0]);
                                    object ValueCheck = check.GetValue(itemdata, null);
                                    var arrValue = Regex.Split(arr[ValueCheck.ToString().ToLower() == arr[1] ? 2 : 3], ":");
                                    if (arrValue[0] != "0")
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.Value = arrValue[0];
                                        cell.EntireRow.Height = height;
                                    }
                                    else
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.Value = arrValue[1];
                                        cell.EntireRow.Height = height;
                                    }
                                    #endregion
                                }
                                if (item == "0")
                                {
                                    var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                    cell.Value = 0;
                                    cell.EntireRow.Height = height;
                                }
                                else
                                {
                                    PropertyInfo property = type.GetProperty(item);
                                    object obj2 = property.GetValue(itemdata, null);
                                    if (obj2 != null && obj2.ToString() != "")
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.EntireRow.Height = height;
                                        if (obj2.ToString().IndexOf("<b>") == 0)
                                        {
                                            cell.Style.Font.Bold = true;
                                            obj2 = obj2.ToString().Substring(3);
                                        }
                                        cell.Value = obj2;
                                    }
                                }
                            }
                            iCol++;
                        }
                        rowIndex++;
                        stt++;
                    }
                    m_objExcelWorksheet.DeleteRow(i_intStartRow);
                }
                SaveFile();
                return m_strOutputFileName;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }
        public string Export3ExcelByList(List<ExcelByKeyModel> lstData, int i_intStartRow, int i_intStartCol, string[] lstColumn)
        {
            try
            {
                var result = new MemoryStream();
                if (!m_init_successful) return "";
                if (lstData != null && lstData.Count() > 0)
                {
                    int stt = 1;
                    int startRow = i_intStartRow;
                    foreach (var data in lstData)
                    {
                        m_objExcelWorksheet.InsertRow(startRow + 1, 1, startRow);
                        startRow = startRow + 1;
                        var addresMerge = m_objExcelWorksheet.Cells[startRow, 1, startRow, i_intStartCol + lstColumn.Length].Address;
                        m_objExcelWorksheet.Cells[addresMerge].Merge = true;
                        m_objExcelWorksheet.Cells[addresMerge].Value = data.Key;
                        m_objExcelWorksheet.Cells[addresMerge].Style.Font.Bold = true;
                        m_objExcelWorksheet.Cells[addresMerge].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        m_objExcelWorksheet.InsertRow(startRow + 1, data.lstData.Count(), startRow);
                        int rowIndex = 1;
                        foreach (var itemdata in data.lstData)
                        {
                            int iCol = 1;
                            Type type = itemdata.GetType();
                            m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol].Value = stt;

                            foreach (string item in lstColumn)
                            {
                                if (item == "")
                                {
                                    var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                    cell.Value = "";
                                }
                                else
                                {
                                    var arr = Regex.Split(item, ",");

                                    if (arr.Length > 1)
                                    {
                                        #region Truong hop truyen vao kieu so sanh
                                        PropertyInfo check = type.GetProperty(arr[0]);
                                        object ValueCheck = check.GetValue(itemdata, null);
                                        var arrValue = Regex.Split(arr[ValueCheck.ToString().ToLower() == arr[1] ? 2 : 3], ":");
                                        if (arrValue[0] != "0")
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Value = arrValue[0];
                                            cell.Style.Font.Bold = false;
                                        }
                                        else
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Value = arrValue[1];
                                            cell.Style.Font.Bold = false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        PropertyInfo property = type.GetProperty(item);
                                        object obj2 = property.GetValue(itemdata, null);
                                        if (obj2 != null && obj2.ToString() != "")
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Style.Font.Bold = false;
                                            if (obj2.ToString().IndexOf("<b>") == 0)
                                            {
                                                cell.Style.Font.Bold = true;
                                                obj2 = obj2.ToString().Substring(3);
                                            }
                                            cell.Value = obj2;
                                        }
                                    }
                                }
                                iCol++;
                            }
                            rowIndex++;
                            stt++;
                        }
                        startRow += data.lstData.Count();
                    }
                    m_objExcelWorksheet.DeleteRow(i_intStartRow);
                }
                SaveFile();
                return m_strOutputFileName;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }

        public MemoryStream Export2ExcelByList(IEnumerable<object> data, int i_intStartRow, int i_intStartCol, string[] lstColumn)
        {
            try
            {
                var result = new MemoryStream();
                if (!m_init_successful) return result;
                if (data != null && data.Count() > 0)
                {
                    if (lstColumn == null)
                        lstColumn = data.FirstOrDefault().GetType().GetProperties().Select(x => x.Name).ToArray<string>();

                    m_objExcelWorksheet.InsertRow(i_intStartRow + 1, data.Count(), i_intStartRow);
                    int rowIndex = 1;
                    int stt = 1;
                    foreach (var itemdata in data)
                    {
                        Type type = itemdata.GetType();
                        m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Value = stt;
                        // ThaySTT để thay vào cột STT vào reset lại số TT = 1
                        if (type.GetProperty("ThaySTT") != null)
                        {
                            PropertyInfo property1 = type.GetProperty("ThaySTT");
                            object obj1 = property1.GetValue(itemdata, null);
                            if (obj1 != null && obj1.ToString() != "")
                            {
                                m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Value = obj1;
                                m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol].Style.Font.Bold = true;
                                stt = 0;
                            }
                        }
                        int iCol = 1;
                        foreach (string item in lstColumn)
                        {
                            if (item == "")
                            {
                                var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                cell.Value = "";
                            }
                            else
                            {
                                var arr = Regex.Split(item, ",");

                                if (arr.Length > 1)
                                {
                                    #region Truong hop truyen vao kieu so sanh
                                    PropertyInfo check = type.GetProperty(arr[0]);
                                    object ValueCheck = check.GetValue(itemdata, null);
                                    var arrValue = Regex.Split(arr[ValueCheck.ToString().ToLower() == arr[1] ? 2 : 3], ":");
                                    if (arrValue[0] != "0")
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.Value = arrValue[0];
                                    }
                                    else
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.Value = arrValue[1];
                                    }
                                    #endregion
                                }
                                else
                                {
                                    if (item == "0")
                                    {
                                        var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                        cell.Value = 0;
                                    }
                                    else
                                    {
                                        PropertyInfo property = type.GetProperty(item);
                                        object obj2 = property.GetValue(itemdata, null);
                                        if (obj2 != null && obj2.ToString() != "")
                                        {
                                            var cell = m_objExcelWorksheet.Cells[i_intStartRow + rowIndex, i_intStartCol + iCol];
                                            if (obj2.ToString().IndexOf("<b>") == 0)
                                            {
                                                cell.Style.Font.Bold = true;
                                                obj2 = obj2.ToString().Substring(3);
                                            }
                                            cell.Value = obj2;
                                        }
                                    }
                                }
                            }
                            iCol++;
                        }
                        rowIndex++;
                        stt++;
                    }
                    m_objExcelWorksheet.DeleteRow(i_intStartRow);
                }
                result = new MemoryStream(pck.GetAsByteArray()); //Get updated stream
                return result;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }
        /// <summary>
        /// Render excel theo 2 lv: 
        /// Danh sách loại - Merge full column
        /// Các phần tử trong loại
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i_intStartRow"></param>
        /// <param name="i_intStartCol"></param>
        /// <param name="lstColumn"></param>
        /// <returns></returns>
        public MemoryStream Export2ExcelByList1(List<ExcelByKeyModel> lstData, int i_intStartRow, int i_intStartCol, string[] lstColumn)
        {
            try
            {
                var result = new MemoryStream();
                if (!m_init_successful) return result;
                if (lstData != null && lstData.Count() > 0)
                {
                    int stt = 1;
                    int startRow = i_intStartRow;
                    foreach (var data in lstData)
                    {
                        m_objExcelWorksheet.InsertRow(startRow + 1, 1, startRow);
                        startRow = startRow + 1;
                        var addresMerge = m_objExcelWorksheet.Cells[startRow, 1, startRow, i_intStartCol + lstColumn.Length].Address;
                        m_objExcelWorksheet.Cells[addresMerge].Merge = true;
                        m_objExcelWorksheet.Cells[addresMerge].Value = data.Key;
                        m_objExcelWorksheet.Cells[addresMerge].Style.Font.Bold = true;
                        m_objExcelWorksheet.Cells[addresMerge].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        m_objExcelWorksheet.InsertRow(startRow + 1, data.lstData.Count(), startRow);
                        int rowIndex = 1;
                        foreach (var itemdata in data.lstData)
                        {
                            int iCol = 1;
                            Type type = itemdata.GetType();
                            m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol].Value = stt;

                            foreach (string item in lstColumn)
                            {
                                if (item == "")
                                {
                                    var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                    cell.Value = "";
                                }
                                else
                                {
                                    var arr = Regex.Split(item, ",");

                                    if (arr.Length > 1)
                                    {
                                        #region Truong hop truyen vao kieu so sanh
                                        PropertyInfo check = type.GetProperty(arr[0]);
                                        object ValueCheck = check.GetValue(itemdata, null);
                                        var arrValue = Regex.Split(arr[ValueCheck.ToString().ToLower() == arr[1] ? 2 : 3], ":");
                                        if (arrValue[0] != "0")
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Value = arrValue[0];
                                            cell.Style.Font.Bold = false;
                                        }
                                        else
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Value = arrValue[1];
                                            cell.Style.Font.Bold = false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        PropertyInfo property = type.GetProperty(item);
                                        object obj2 = property.GetValue(itemdata, null);
                                        if (obj2 != null && obj2.ToString() != "")
                                        {
                                            var cell = m_objExcelWorksheet.Cells[startRow + rowIndex, i_intStartCol + iCol];
                                            cell.Style.Font.Bold = false;
                                            if (obj2.ToString().IndexOf("<b>") == 0)
                                            {
                                                cell.Style.Font.Bold = true;
                                                obj2 = obj2.ToString().Substring(3);
                                            }
                                            cell.Value = obj2;
                                        }
                                    }
                                }
                                iCol++;
                            }
                            rowIndex++;
                            stt++;
                        }
                        startRow += data.lstData.Count();
                    }
                    m_objExcelWorksheet.DeleteRow(i_intStartRow);
                }
                result = new MemoryStream(pck.GetAsByteArray()); //Get updated stream
                return result;
            }
            catch (Exception v_e)
            {
                pck.Save();
                throw v_e;
            }
        }

        /// <summary>
        /// Lưu file kết quả kết xuất
        /// </summary>
        /// <returns></returns>
        public string SaveFile()
        {
            pck.Save();
            return m_strOutputFileName;
        }
        /// <summary>
        /// Lấy thông tin file excel kết xuất ra
        /// </summary>
        public string OutputFileName
        {
            get { return m_strOutputFileName; }
        }
        public string saveFileExcel(MemoryStream stream, string path, string namefile)
        {

            var fullpath = HostingEnvironment.MapPath(path + namefile);
            var existingFile = new FileInfo(fullpath);
            if (existingFile.Exists)
                existingFile.Delete();

            var pack = new ExcelPackage();
            pack.Stream.CopyTo(stream);
            pack.Load(stream);
            pack.SaveAs(existingFile);

            //cleanup
            pack.Dispose();
            stream.Dispose();

            return (path + namefile);
        }
    }
}
