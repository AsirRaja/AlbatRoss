using AlbatRoss.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace AlbatRoss.Controllers
{
    public static class CommonUtil
    {
        public static DataConnection DB = new DataConnection();
        public static string wwwrootDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        public static string Date_Format = "dd-MM-yyyy";
        public static string Date_Time_Format = "dd-MM-yyyy HH:mm";
        public static string MySQL_Date_Format = "yyyy-MM-dd";
        public static string MySQL_Date_Time_Format = "yyyy-MM-dd HH:mm:ss";
        public static List<SelectListItem> GetEmployees()
        {
            string query = "select Empid, EmployeeName from userlogin;";
            var data = DB.ReturnCommand(query);
            List<SelectListItem> employee = new List<SelectListItem>();
            foreach (DataRow item in data.Rows)
            {
                employee.Add(new SelectListItem { Text = item.ItemArray[1].ToString(), Value = item.ItemArray[0].ToString() });
            }
            return employee;
        }
        public static List<SelectListItem> AllRate()
        {
            string query = "select * from allrate";
            var data = DB.ReturnCommand(query);
            List<SelectListItem> str = new List<SelectListItem>();
            str.Add(new SelectListItem { Text = "Select...", Value = "" });
            foreach (DataRow item in data.Rows)
            {
                str.Add(new SelectListItem { Text = item.ItemArray[1].ToString(), Value = "" });
            }
            return str;
        }
        public static string ToMySQLDateTimeFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToMySQLDateFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static DateTime ToVisualStudioDateFormat(this string date)
        {
            if (date.Contains("/"))
                date = date.Replace('/', '-');
            if (!date.Contains(" "))
            {
                date = date + " " + "00:00:00";
            }
            if (date.Contains("AM") || date.Contains("PM"))
            {
                DateTime time = Convert.ToDateTime(date);
                return time;
            }
            return DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static T[] ToEnumCollection<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        public enum RateColumns
        {
            [Description("Id")]
            Id,
            [Description("Service Name")]
            ProductName,
            [Description("Price")]
            Rate,
            [Description("MemberShip Price")]
            MemberRate,
            [Description("Offer Price")]
            OfferRate,
            [Description("")]
            DeleteRow
        }
        public enum DateFormats
        {
            [Description("M/d/yyyy")]
            Format1,
            [Description("M/d/yy")]
            Format2,
            [Description("MM/dd/yy")]
            Format3,
            [Description("MM/dd/yyyy")]
            Format4,
            [Description("yy/MM/dd")]
            Format5,
            [Description("yyyy-MM-dd")]
            Format6,
            [Description("dd-MMM-yy")]
            Format7,
            [Description("dd-MMM-yyyy")]
            Format8,
            [Description("dd-MM-yyyy")]
            Format9,
        }
        public static string SecondsToHours(int seconds)
        {
            if (seconds == 0)
                return "00:00:00";
            return new TimeSpan(0, 0, seconds).ToString();
        }
        public static string MinutesToHours(int minutes)
        {
            string hours = "";
            int totalHour = (minutes) / 60;
            int totalMinutes = (minutes) % 60;

            if (Convert.ToString(totalHour).StartsWith("-"))
            {
                hours = "-" + totalHour.ToString("00").Remove(0, 1) + ":" + totalMinutes.ToString("00").Replace("-", "");
            }
            else if (Convert.ToString(totalMinutes).StartsWith("-"))
            {
                hours = "-" + totalHour.ToString("00").Replace("-", "") + ":" + totalMinutes.ToString("00").Remove(0, 1);
            }
            else
            {
                hours = totalHour.ToString("00") + ":" + totalMinutes.ToString("00");
            }
            return hours;
        }
        public static int HoursToMinutes(string hours)
        {

            int hrs = 0;
            int min = 0;
            string[] temp = hours == null ? "0".Split(':') : hours.Split(':');
            if (temp[0].Contains('.'))
            {
                string[] tmp = temp[0].Split('.');
                hrs = Convert.ToInt32(tmp[0]) * 24 + Convert.ToInt32(tmp[1]);
            }
            else
            {
                hrs = Convert.ToInt32(temp[0].Trim() == string.Empty ? "0" : temp[0]);
            }

            if (temp.Count() != 1)
            {
                if (temp[1] != "")
                {
                    min = Convert.ToInt32(temp[1]);
                }
            }
            int minutes = hrs * 60 + min;

            return minutes;
        }
    }
    public static class Excel
    {
        public static void FillExcel(XLWorkbook workbook, DataTable data)
        {
            var worksheet = workbook.Worksheets.Add("Sheet1");
            FillExcelCellValue(worksheet, 1, 1, 1, data.Columns.Count, "AlbatRoss Ladies Professional Salon", 16, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, XLColor.Black, XLColor.Transparent, 15, false);
            for (int i = 0; i < data.Columns.Count; i++)
            {
                FillExcelCellValue(worksheet, 2, i + 1, Convert.ToString(data.Columns[i]), 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, XLColor.Black, XLColor.Transparent, 15, false);
            }
            int start = 3;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    FillExcelCellValue(worksheet, start, j + 1, data.Rows[i][j].ToString(), 12, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, XLColor.Black, XLColor.Transparent, 15, false);
                }
                start++;
            }
        }
        public static void FillExcelCellValue(IXLWorksheet workSheet, int x, int y, string value, double fontSize, bool bold, XLAlignmentHorizontalValues Horizontal, XLAlignmentVerticalValues Vertical, XLColor foreColor, XLColor backColor, int columWidth, bool wrap)
        {
            workSheet.Cell(x, y).Value = value;
            workSheet.Cell(x, y).Style.Font.Bold = bold;
            workSheet.Cell(x, y).Style.Alignment.Horizontal = Horizontal;
            workSheet.Cell(x, y).Style.Alignment.Vertical = Vertical;
            workSheet.Cell(x, y).Style.Font.FontColor = foreColor;
            workSheet.Cell(x, y).Style.Fill.BackgroundColor = backColor;
            workSheet.Cell(x, y).Style.Font.FontSize = fontSize;
            workSheet.Columns(x, y - 1).Width = value.Length + columWidth;
            workSheet.Cell(x, y).Style.Alignment.WrapText = wrap;
            workSheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;     //All Borders Set
            //workSheet.Column(y).AdjustToContents();                                       //All Column Auto Adjustment
        }
        public static void FillExcelCellValue(IXLWorksheet workSheet, int x, int y, int x1, int y1, string value, double fontSize, bool bold, XLAlignmentHorizontalValues Horizontal, XLAlignmentVerticalValues Vertical, XLColor foreColor, XLColor backColor, int columWidth, bool wrap)
        {
            workSheet.Range(workSheet.Cell(x, y), workSheet.Cell(x1, y1)).Merge();  // (1, 1) = top - left  merge  // (2,3) = bottom - right of merge
            workSheet.Cell(x, y).Value = value;
            workSheet.Cell(x, y).Style.Font.Bold = bold;
            workSheet.Cell(x, y).Style.Alignment.Horizontal = Horizontal;
            workSheet.Cell(x, y).Style.Alignment.Vertical = Vertical;
            workSheet.Cell(x, y).Style.Font.FontColor = foreColor;
            workSheet.Cell(x, y).Style.Fill.BackgroundColor = backColor;
            workSheet.Cell(x, y).Style.Font.FontSize = fontSize;
            workSheet.Columns(x, y).Width = value.Length + columWidth;
            workSheet.Cell(x, y).Style.Alignment.WrapText = wrap;
            workSheet.Cell(x1, y1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;   //All Borders Set
            //workSheet.Rows(x, y).Height = rowHeight;
        }
    }
}
