using Albat.Models;
using AlbatRoss.Controllers;
using AlbatRoss.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Albat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DataConnection database = new DataConnection();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Service(string id)
        {
            ServiceModel model = new ServiceModel();
            model.Service = new List<SelectListItem>();
            model.ProductName = id;
            string macPrefix = string.Empty;
            model.ServiceCode = database.GenerateID("Prefix", ref macPrefix, "1");
            if (id != null)
            {
                model.ProductName = id.Contains("~") ? id.Split('~')[1] : id;
                ViewData["idChecked"] = id.Contains("~") ? id.Split('~')[1] : id;
            }
            model.Items = CommonUtil.AllRate();
            model.Employee = CommonUtil.GetEmployees();
            return View(model);
        }
        public JsonResult Services(string id)
        {
            try
            {
                string values = database.GetAllRate(id);
                string newRow = string.Empty;
                newRow = "<tr><td style='height: 10px;text-align: center;'>" + values.Split("~")[0] + "</td><td style='height: 10px;text-align: center;' contenteditable='true' oninput='myFunction1(this)' id='qty'></td>" +
                        "<td id='prices' style='height: 10px;text-align: center;'>" + values.Split("~")[1] + "</td>" + "<td id='tot' style='height: 10px;text-align: center;'></td>" +
                        "<td style='height: 10px;text-align: center;'><label onclick='removeRow(this)'><i class='fa fa-trash-o' style='font-size:26px;color:red;margin:10px'></i></label></td></tr>";
                return Json(newRow);
            }
            catch (Exception ex)
            {
                return Json("false");
            }
        }
        [HttpPost]
        public IActionResult SaveDatas([FromBody] List<ServiceModel> gridData)
        {
            string macPrefix = string.Empty;
            string values = string.Empty;
            if (gridData != null)
            {
                foreach (var model in gridData)
                {
                    values = database.ServiceSave1(model);
                }
            }
            if (values == "Success")
            {
                string prefixNo = database.GenerateID("Prefix", ref macPrefix, "1");
                database.UpdatePrefix(prefixNo, "ALB", "1");
            }
            return Json("");
        }
        public IActionResult Clients()
        {
            ClientModel model = new ClientModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Clients(ClientModel model)
        {
            model.ClientDetails = database.GetClientDetails(model);
            return View(model);
        }
        public IActionResult DailyLog()
        {
            DailyLogModel model = new DailyLogModel();
            model.DailyLog = database.DailyLogDetails(model);
            decimal grandtotal = 0;decimal total = 0;
            foreach (var item in model.DailyLog)
            {
                grandtotal += Convert.ToDecimal(item.Total);
            }
            
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now;
            model.GrandTotal = Convert.ToString(grandtotal);
            model.Employees = CommonUtil.GetEmployees();
            return View(model);
        }
        [HttpPost]
        public IActionResult DailyLog(DailyLogModel model)
        {
            model.DailyLog = database.DailyLogDetails(model);
            decimal grandtotal = 0;
            foreach (var item in model.DailyLog)
            {
                grandtotal += Convert.ToDecimal(item.Total);
            }
            model.GrandTotal = Convert.ToString(grandtotal);
            model.Employees = CommonUtil.GetEmployees();
            return View(model);
        }
        public IActionResult MemberShip(string id)
        {
            MemberShipModel member = new MemberShipModel();
            member.MemberList = database.GetmemberShipData(id);
            member.JoinDate = DateTime.Now;
            member.FinishDate = DateTime.Now.AddYears(1);
            string macPrefix = string.Empty;
            member.MemberShipNo = database.GenerateID("Prefix", ref macPrefix, "2");
            ViewData["id"] = id;
            return View(member);
        }
        public JsonResult MemberShipDetails(string id)
        {
            string macPrefix = string.Empty;
            database.SaveMemberShipData(id);
            string values = database.GenerateID("Prefix", ref macPrefix, "2");
            database.UpdatePrefix(values, "MEM", "2");
            return Json("Save");
        }
        public IActionResult Menu()
        {
            return View();
        }
        public string GenerateIds(string id)
        {
            string macPrefix = string.Empty;
            string profixid = database.GenerateID("Prefix", ref macPrefix, id);
            return profixid;
        }
        public IActionResult MonthlyReport()
        {
            MonthlyCost model = new MonthlyCost();
            model.stock = GetMonthlyCostModel(model);
            List<int> years = new List<int>();
            int currentYear = DateTime.Now.Year;
            for(int i = 0; i < 5; i++)
            {
                years.Add(currentYear - i);
            }
            ViewBag.Years = years;
            return View(model);
        }
        public List<MonthlyCost> GetMonthlyCostModel(MonthlyCost model)
        {
            List<MonthlyCost> stockList = new List<MonthlyCost>();
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string query = "select Count(id)as Counts,SUM(REPLACE(Total, ',', '')) as Total,month(currenDate) Month,year(currenDate)as Year from tblservice where year(currenDate)='" + DateTime.Now.Year +"' group by month(currenDate),year(currenDate)";
            var dt = database.ReturnCommand(query);
            MonthlyCost stock = new MonthlyCost();
            for (int i = 0; i < 12; i++)
            {
                stock = new MonthlyCost();
                stock.Month = months[i];
                int monthDay = i + 1;
                string mm = monthDay.ToString();
                if (dt.Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow firstRow in dt.Rows)
                    {
                        if (firstRow["Month"].ToString() == mm)
                        {
                            stock.Services = firstRow["Counts"].ToString();
                            stock.Year = firstRow["Year"].ToString();
                            stock.Total = firstRow["Total"].ToString() == "" ? "0" : firstRow["Total"].ToString();
                            stockList.Add(stock);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        stock.Services = "0";
                        stock.Total = "0";
                        stockList.Add(stock);
                        count++;
                    }
                }
                else
                {
                    stock.Month = "0";
                    stock.Total = "0";
                    stockList.Add(stock);
                }
                //stockList.Add(stock);
            }
            return stockList;
        }
        public JsonResult GetYears()                                 //ExcelSheet FDownload
        {
            List<int> years = new List<int>();
            int currentYear = DateTime.Now.Year;
            for (int i = 0; i < 5; i++)
            {
                years.Add(currentYear - i);
            }
            return Json(years.AsEnumerable().Select(c => new { year = c }));
        }
        [HttpPost]
        public IActionResult ExcelReport(string start, string end)
        {
            System.Data.DataTable data = database.ReturnCommand("select ClientName, Contact, ServiceName, Price, ServiceDate, Qty, Total from tblservice where Date(ServiceDate) between '" + CommonUtil.ToMySQLDateFormat(Convert.ToDateTime(start)) + "' and '" + CommonUtil.ToMySQLDateFormat(Convert.ToDateTime(end)) + "'");
            string file = CommonUtil.wwwrootDir + "/Report/Report.xlsx";                    //Current Path
            if (System.IO.File.Exists(file))                                                //Already File iruka illayanu check panna
                System.IO.File.Delete(file);
            XLWorkbook workbook = new XLWorkbook();
            Excel.FillExcel(workbook, data);
            var memory = new MemoryStream();
            workbook.SaveAs(memory);
            var content = memory.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SimpleExcel.xlsx");
        }
    }
}
