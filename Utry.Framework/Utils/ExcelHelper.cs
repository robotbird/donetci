using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;

using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data.OleDb;
using NPOI.XSSF.UserModel;
namespace Utry.Framework.Utils
{
    public class ExcelHelper
    {
        public enum ExportFormat
        {
            /// <summary>
            /// XLS
            /// </summary>
            XLS,
            /// <summary>
            /// CSV
            /// </summary>
            CSV,
            /// <summary>
            /// DOC
            /// </summary>
            DOC,
            /// <summary>
            /// TXT
            /// </summary>
            TXT
        }
                 /// <summary>
          /// DataTable导出到Excel文件
          /// </summary>
          /// <param name="dtSource">源DataTable</param>
          /// <param name="strHeaderText">表头文本</param>
          /// <param name="strFileName">保存位置</param>
          public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
          {
              using (MemoryStream ms = Export(dtSource, strHeaderText))
              {
                  using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                  {
                      byte[] data = ms.ToArray();
                      fs.Write(data, 0, data.Length);
                      fs.Flush();
                  }
              }
          }
  
          /// <summary>
          /// DataTable导出到Excel的MemoryStream
          /// </summary>
          /// <param name="dtSource">源DataTable</param>
          /// <param name="strHeaderText">表头文本</param>
          public static MemoryStream Export(DataTable dtSource, string strHeaderText)
          {
              HSSFWorkbook workbook = new HSSFWorkbook();
              HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
              #region 右击文件 属性信息
              {
                  DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                  dsi.Company = "utry.cn";
                  workbook.DocumentSummaryInformation = dsi;
                  SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                  si.Author = "utry"; //填加xls文件作者信息
                  si.ApplicationName = "ots"; //填加xls文件创建程序信息
                  si.LastAuthor = "utry"; //填加xls文件最后保存者信息
                  si.Comments = "说明信息"; //填加xls文件作者信息
                  si.Title = "ots-excel"; //填加xls文件标题信息
                  si.Subject = "ots-excel";//填加文件主题信息
                  si.CreateDateTime = DateTime.Now;
                  workbook.SummaryInformation = si;
              }
              #endregion
              HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
              HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
              dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
              //取得列宽
              int[] arrColWidth = new int[dtSource.Columns.Count];
              foreach (DataColumn item in dtSource.Columns)
              {
                  arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
              }
              for (int i = 0; i < dtSource.Rows.Count; i++)
              {
                  for (int j = 0; j < dtSource.Columns.Count; j++)
                  {
                      int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                      if (intTemp > arrColWidth[j])
                      {
                          arrColWidth[j] = intTemp;
                      }
                  }
              }
              int rowIndex = 0;

              #region 新建表，填充表头，填充列头，样式
              if (rowIndex == 65535 || rowIndex == 0)
              {
                  if (rowIndex != 0)
                  {
                      sheet = workbook.CreateSheet() as HSSFSheet;
                  }
                  #region 表头及样式
                  {
                      HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                      headerRow.HeightInPoints = 25;
                      headerRow.CreateCell(0).SetCellValue(strHeaderText);
                      HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                      headStyle.Alignment = HorizontalAlignment.CENTER;
                      HSSFFont font = workbook.CreateFont() as HSSFFont;
                      font.FontHeightInPoints = 20;
                      font.Boldweight = 700;
                      headStyle.SetFont(font);
                      headerRow.GetCell(0).CellStyle = headStyle;
                      sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                  }
                  #endregion

                  #region 列头及样式
                  {
                      HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                      HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                      headStyle.Alignment = HorizontalAlignment.CENTER;
                      HSSFFont font = workbook.CreateFont() as HSSFFont;
                      font.FontHeightInPoints = 10;
                      font.Boldweight = 700;
                      headStyle.IsLocked = true;
                      headStyle.SetFont(font);
                      foreach (DataColumn column in dtSource.Columns)
                      {
                          headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                          headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                          //设置列宽
                          int cwidth = (arrColWidth[column.Ordinal] + 1);
                          if (cwidth >= 255)
                          {
                              cwidth = 255;
                          }
                          int width = cwidth * 256;
                          sheet.SetColumnWidth(column.Ordinal, width);

                      }
                      sheet.CreateFreezePane(0, 2, 0, dtSource.Columns.Count - 1);
                  }
                  #endregion
                  rowIndex = 2;
              }
              #endregion

              foreach (DataRow row in dtSource.Rows)
              {
                 #region 填充内容
                 HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                 foreach (DataColumn column in dtSource.Columns)
                 {
                     HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                     string drValue = row[column].ToString();
                     switch (column.DataType.ToString())
                     {
                         case "System.String"://字符串类型
                             newCell.SetCellValue(drValue);
                             break;
                         case "System.DateTime"://日期类型
                             DateTime dateV;
                             DateTime.TryParse(drValue, out dateV);
                             newCell.SetCellValue(dateV);
 
                             newCell.CellStyle = dateStyle;//格式化显示
                             break;
                         case "System.Boolean"://布尔型
                             bool boolV = false;
                             bool.TryParse(drValue, out boolV);
                             newCell.SetCellValue(boolV);
                             break;
                         case "System.Int16"://整型
                         case "System.Int32":
                         case "System.Int64":
                         case "System.Byte":
                             int intV = 0;
                             int.TryParse(drValue, out intV);
                             newCell.SetCellValue(intV);
                             break;
                         case "System.Decimal"://浮点型
                         case "System.Double":
                             double doubV = 0;
                             double.TryParse(drValue, out doubV);
                             newCell.SetCellValue(doubV);
                             break;
                         case "System.DBNull"://空值处理
                             newCell.SetCellValue("");
                             break;
                         default:
                             newCell.SetCellValue("");
                             break;
                     }
                 }

                 #endregion
                 rowIndex++;
             }
              //统计功能
              int count = dtSource.Rows.Count;//总共条数
              HSSFRow bottomRow = sheet.CreateRow(count+2) as HSSFRow;
              HSSFCell bottomCell1 = (HSSFCell)bottomRow.CreateCell(0);
              HSSFCell bottomCell2 = (HSSFCell)bottomRow.CreateCell(1);
              bottomCell1.SetCellValue("合计：");
              bottomCell2.SetCellValue(count + "条");

             using (MemoryStream ms = new MemoryStream())
             {
                 workbook.Write(ms);
                 ms.Flush();
                 ms.Position = 0;
                 return ms;
             }
         }
 
 
         /// <summary>
         /// 用于Web导出
         /// </summary>
         /// <param name="dtSource">源DataTable</param>
         /// <param name="strHeaderText">表头文本</param>
         /// <param name="strFileName">文件名</param>
         public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
         {
             HttpContext curContext = HttpContext.Current;
             // 设置编码和附件格式
             curContext.Response.ContentType = "application/vnd.ms-excel";
             curContext.Response.ContentEncoding = Encoding.UTF8;
             curContext.Response.Charset = "";
             curContext.Response.AppendHeader("Content-Disposition",
                 "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
             curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
             curContext.Response.End();
         }
 
 
         /// <summary>读取excel
         /// 默认第一行为标头
         /// </summary>
         /// <param name="strFileName">excel文档路径</param>
         /// <returns></returns>
         public static DataTable Import(string strFileName)
         {

             if (strFileName.ToLower().IndexOf(".xlsx")!=-1)
             {
               return Import2007(strFileName);
             }

             DataTable dt = new DataTable();
             HSSFWorkbook hssfworkbook;
             using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
             {
                 hssfworkbook = new HSSFWorkbook(file);
             }
             HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
             System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
             HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
             int cellCount = headerRow.LastCellNum;
             for (int j = 0; j < cellCount; j++)
             {
                 HSSFCell cell = headerRow.GetCell(j) as HSSFCell;
                 dt.Columns.Add(cell.ToString());
             }
             for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
             {
                 HSSFRow row = sheet.GetRow(i) as HSSFRow;
                 DataRow dataRow = dt.NewRow();
                 for (int j = row.FirstCellNum; j < cellCount; j++)
                 {
                     if (row.GetCell(j) != null)
                         dataRow[j] = row.GetCell(j).ToString();
                 }
                 dt.Rows.Add(dataRow);
             }
             return dt;
        }

         /// <summary>读取excel2007
         /// 默认第一行为标头
         /// </summary>
         /// <param name="strFileName">excel文档路径</param>
         /// <returns></returns>
         private static DataTable Import2007(string strFileName)
         {
             //2013/10/22 夏梁峰 add begin
             //修改使用NPOI读取excel
             //需求编号：OTS_SZDX_01_R00027_D00001
             XSSFWorkbook hssfworkbook;
             using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
             {
                 hssfworkbook = new XSSFWorkbook(file);
             }

             DataTable dt = new DataTable();
             ISheet sheet = hssfworkbook.GetSheetAt(0);
             System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
             XSSFRow headerRow = sheet.GetRow(0) as XSSFRow;
             int cellCount = headerRow.LastCellNum;
             for (int j = 0; j < cellCount; j++)
             {
                 XSSFCell cell = headerRow.GetCell(j) as XSSFCell;
                 dt.Columns.Add(cell.ToString());
             }
             for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
             {
                 XSSFRow row = sheet.GetRow(i) as XSSFRow;
                 DataRow dataRow = dt.NewRow();
                 for (int j = row.FirstCellNum; j < cellCount; j++)
                 {
                     if (row.GetCell(j) != null)
                         dataRow[j] = row.GetCell(j).ToString();
                 }
                 dt.Rows.Add(dataRow);
             }
             return dt;
             //2013/10/22 夏梁峰 add end

             //2013/10/22 夏梁峰 删除
             //string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFileName + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
             //OleDbConnection myConn = new OleDbConnection(strCon);
             //string strCom = " SELECT * FROM [Sheet1$]";
             //myConn.Open();
             //OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
             //DataSet myDataSet = new DataSet();
             //myCommand.Fill(myDataSet, "[Sheet1$]");
             //myConn.Close();
             //return myDataSet.Tables[0];
         }

         /// <summary>
         /// 用于Web导出
         /// </summary>
         /// <param name="dtSource">源DataTable</param>
         /// <param name="strHeaderText">表头文本</param>
         /// <param name="strFileName">文件名</param>
         public static void ExportByCSV(DataTable dtSource, string strHeaderText, string strFileName)
         {
             HttpResponse resp = HttpContext.Current.Response;
             // 设置编码和附件格式
             resp.ContentType = "application/vnd.ms-excel";
             resp.ContentEncoding = Encoding.UTF8;
             resp.Charset = "";
             resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

             string colHeaders = "";
             int colCount = dtSource.Columns.Count;
             var dtCol = new DataColumn();

             for (int i = 0; i < colCount; i++)
             {
                 dtCol = dtSource.Columns[i];
                 colHeaders = colHeaders + dtCol.ColumnName + ",";

             }
             if (colHeaders.Length > 0)
             {
                 colHeaders = colHeaders.Substring(0, colHeaders.LastIndexOf(','));
             }
             colHeaders = colHeaders + (char)13 + (char)10;
             resp.Write(colHeaders);

             int num = 0;
             StringBuilder sb = new StringBuilder();
             for (int i = 0; i < colCount; i++)
             {
                 if (num < 1000)
                 {
                     sb.Append(Export(dtSource.Rows[i], dtSource));
                     num++;
                 }
                 else
                 {
                     resp.Write(sb.ToString());
                     sb.Remove(0, sb.Length);
                     num = 0;
                 }
             }
             resp.Write(sb.ToString());
             resp.End();

         }


         /// <summary>
         /// DataTable导出到Excel的MemoryStream
         /// </summary>
         /// <param name="dtSource">源DataTable</param>
         /// <param name="strHeaderText">表头文本</param>
         public static string Export(DataRow dr, DataTable dtSource)
         {
             StringBuilder sb = new StringBuilder();
             for (int i = 0; i < dtSource.Rows.Count; i++)
             {
                 foreach (DataColumn column in dtSource.Columns)
                 {
                     if (i == dtSource.Rows.Count - 1)
                     {
                         sb.Append((char)9);
                         sb.Append(dr[column.ColumnName].ToString());
                         sb.Append((char)13);
                         sb.Append((char)10);
                     }
                     else
                     {
                        // sb.Append((char)9);
                         sb.Append(dr[column.ColumnName].ToString() + ",");
                     }

                 }

             }

             return sb.ToString();
         }


         /// <summary>
         /// 由DataTable导出Excel
         /// </summary>
         /// <param name="dt"></param>
         /// <param name="fileName"></param>
         public static void ExportToExcel(DataTable dt, string fileName)
         {
             HttpContext.Current.Response.Clear();
             HttpContext.Current.Response.ClearContent();
             HttpContext.Current.Response.ClearHeaders();
             HttpResponse resp = HttpContext.Current.Response;
             // 设置编码和附件格式
             resp.ContentType = "application/vnd.ms-excel";
             resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
             resp.Charset = "";
             resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
             string colHeaders = @"", ls_item = "";

             ////定义表对象与行对象，同时用DataSet对其值进行初始化
             //DataTable dt = ds.Tables[0];
             DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的
             int i = 0;
             int cl = dt.Columns.Count;

             //取得数据表各列标题，各标题之间以t分割，最后一个列标题后加回车符
             for (i = 0; i < cl; i++)
             {
                 if (i == (cl - 1))//最后一列，加n
                 {
                     colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                 }
                 else
                 {
                     colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                 }

             }
             resp.Write(colHeaders);
             //向HTTP输出流中写入取得的数据信息

             //逐行处理数据 
             foreach (DataRow row in myRow)
             {
                 //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据 
                 for (i = 0; i < cl; i++)
                 {
                     if (i == (cl - 1))//最后一列，加n
                     {
                         if (ValidateHelper.IsNumeric(row[i]) && row[i].ToString().Length > 11 || row[i].ToString().IndexOf("755") ==0)
                         {
                             //ls_item += ";" + row[i].ToString().Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\n";
                             ls_item += "=\"" + row[i].ToString()+"\"".Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\n";
                         }
                         else 
                         {
                             ls_item += row[i].ToString().Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\n";                         
                         }
                     }
                     else
                     {
                         if (ValidateHelper.IsNumeric(row[i]) && row[i].ToString().Length > 11 || row[i].ToString().IndexOf("755")==0)
                         {
                            // ls_item +=";" +row[i].ToString().Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\t";
                             ls_item += "=\"" + row[i].ToString()+"\"".Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\t";
                         }
                         else 
                         {
                             ls_item += row[i].ToString().Replace("\t", "").Replace("\r", "").Replace("\n", "") + "\t";                         
                         }
                     }

                 }
                 resp.Write(ls_item);
                 ls_item = "";

             }
             resp.End();
         }


        public static void ExporttoExcel2(DataTable table,string filename)
{
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            var sb = new StringBuilder();

            HttpContext.Current.Response.ContentType = "application/ms-excel";
            sb.Append(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + " ");
          
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
              //sets font

            
            sb.Append("<font style='font-size:10.0pt; font-family:Calibri;'>");
            sb.Append("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;
 
            for (int j = 0; j < columnscount; j++)
            {
             sb.Append("<Td>");
            //Makes headers bold
            sb.Append("<B>");
            sb.Append(table.Columns[j].Caption.ToString());
            sb.Append("</B>");
            sb.Append("</Td>");
           }
        sb.Append("</TR>");
        foreach (DataRow row in table.Rows)
        {
            sb.Append("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.Append("<Td style=\"vnd.ms-excel.numberformat:@\">");
                sb.Append(row[i].ToString());
                sb.Append("</Td>");
            }
            sb.Append("</TR>");
        }
        sb.Append("</Table>");
        sb.Append("</font>");
        HttpContext.Current.Response.Write(sb.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

        public static void ExporttoExcel3(DataTable table, string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            var sb = new StringBuilder();

            HttpContext.Current.Response.ContentType = "application/ms-excel";
            sb.Append(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + " ");

            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //sets font


            sb.Append("<font style='font-size:10.0pt;'>");
            sb.Append("<Table border='1'  cellSpacing='0' cellPadding='0' style='font-size:10.0pt; '> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                sb.Append("<Td>");
                //Makes headers bold
                sb.Append("<B>");
                sb.Append(table.Columns[j].Caption.ToString());
                sb.Append("</B>");
                sb.Append("</Td>");
            }
            sb.Append("</TR>");
            foreach (DataRow row in table.Rows)
            {
                sb.Append("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sb.Append("<Td style=\"vnd.ms-excel.numberformat:@\">");
                    sb.Append(row[i].ToString());
                    sb.Append("</Td>");
                }
                sb.Append("</TR>");
            }
            sb.Append("</Table>");
            sb.Append("</font>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
       
        /// <summary>
        ///  替换特殊字符
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static string ReplaceSpecialChars(string input)
        {
            // space 	-> 	_x0020_
            // %	 -> 	_x0025_
            // #	 ->	_x0023_
            // &	 ->	_x0026_
            // /	 ->	_x002F_
            input = input.Replace(" ", "_x0020_")
                  .Replace("%", "_x0025_")
                  .Replace("#", "_x0023_")
                  .Replace("&", "_x0026_")
                  .Replace("/", "_x002F_");
            return input;
        }
        /// <summary>
        /// 导数据源的数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();

            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            string[] headers = new string[dtExport.Columns.Count];
            string[] fields = new string[dtExport.Columns.Count];

            for (int i = 0; i < dtExport.Columns.Count; i++)
            {
                headers[i] = dtExport.Columns[i].ColumnName;
                fields[i] = ReplaceSpecialChars(dtExport.Columns[i].ColumnName);
            }
            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }
        /// <summary>
        /// 导出SmartGridView的数据源的数据
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="headers">导出的表头数组</param>
        /// <param name="fields">导出的字段数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        private static void Export(DataSet ds, string[] headers, string[] fields, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = String.Format("text/{0}", exportFormat.ToString().ToLower());
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}.{1}", fileName, exportFormat.ToString().ToLower()));
            HttpContext.Current.Response.ContentEncoding = encoding;

            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, encoding);

            CreateStylesheet(writer, headers, fields, exportFormat);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            XmlDataDocument xmlDoc = new XmlDataDocument(ds);
            XslCompiledTransform xslTran = new XslCompiledTransform();
            xslTran.Load(new XmlTextReader(stream));

            System.IO.StringWriter sw = new System.IO.StringWriter();
            xslTran.Transform(xmlDoc, null, sw);

            HttpContext.Current.Response.Write(sw.ToString());
            sw.Close();
            writer.Close();
            stream.Close();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 动态生成XSL，并写入XML流
        /// </summary>
        /// <param name="writer">XML流</param>
        /// <param name="headers">表头数组</param>
        /// <param name="fields">字段数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        private static void CreateStylesheet(XmlTextWriter writer, string[] headers, string[] fields, ExportFormat exportFormat)
        {
            string ns = "http://www.w3.org/1999/XSL/Transform";
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("xsl", "stylesheet", ns);
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("xsl:output");
            writer.WriteAttributeString("method", "text");
            writer.WriteAttributeString("version", "4.0");
            writer.WriteEndElement();

            // xsl-template
            writer.WriteStartElement("xsl:template");
            writer.WriteAttributeString("match", "/");

            // xsl:value-of for headers
            for (int i = 0; i < headers.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", "'" + headers[i] + "'");
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != fields.Length - 1) writer.WriteString((exportFormat == ExportFormat.CSV) ? "," : "	");
            }
            // xsl:for-each
            writer.WriteStartElement("xsl:for-each");
            writer.WriteAttributeString("select", "Export/Values");
            writer.WriteString("\r\n");

            // xsl:value-of for data fields
            //<Cell><Data ss:Type="String">
            for (int i = 0; i < fields.Length; i++)
            {
                writer.WriteString("\"");
                writer.WriteStartElement("xsl:value-of");
                writer.WriteAttributeString("select", fields[i]);
                writer.WriteEndElement(); // xsl:value-of
                writer.WriteString("\"");
                if (i != fields.Length - 1) writer.WriteString((exportFormat == ExportFormat.CSV) ? "," : "	");
            }
            // </Data></Cell>
            writer.WriteEndElement(); // xsl:for-each
            writer.WriteEndElement(); // xsl-template
            writer.WriteEndElement(); // xsl:stylesheet
        }       
        /// <summary>
        /// 导出数据源的数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="columnIndexList">导出的列索引数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, int[] columnIndexList, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();

            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);

            string[] headers = new string[columnIndexList.Length];
            string[] fields = new string[columnIndexList.Length];

            for (int i = 0; i < columnIndexList.Length; i++)
            {
                headers[i] = dtExport.Columns[columnIndexList[i]].ColumnName;
                fields[i] = ReplaceSpecialChars(dtExport.Columns[columnIndexList[i]].ColumnName);
            }
            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }
        /// <summary>
        /// 导出数据源的数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="columnIndexList">导出的列索引数组</param>
        /// <param name="headers">导出的列标题数组</param>
        /// <param name="exportFormat">导出文件的格式</param>
        /// <param name="fileName">输出文件名</param>
        /// <param name="encoding">编码</param>
        public static void Export(DataTable dt, int[] columnIndexList, string[] headers, ExportFormat exportFormat, string fileName, Encoding encoding)
        {
            DataSet dsExport = new DataSet("Export");
            DataTable dtExport = dt.Copy();
            dtExport.TableName = "Values";
            dsExport.Tables.Add(dtExport);
            string[] fields = new string[columnIndexList.Length];
            for (int i = 0; i < columnIndexList.Length; i++)
            {
                fields[i] = ReplaceSpecialChars(dtExport.Columns[columnIndexList[i]].ColumnName);
            }
            Export(dsExport, headers, fields, exportFormat, fileName, encoding);
        }        
 

    }


}