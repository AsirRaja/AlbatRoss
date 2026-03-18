using Albat.Models;
using AlbatRoss.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;

namespace AlbatRoss.Controllers
{
    public class MakeUpController : Controller
    {
        DataConnection db = new DataConnection();
        public IActionResult MakeUp()
        {
            return View();
        }
        public string Save(string name, string phone, string place, string date, string time, string amount, string advance, string balance)
        {
            string makeupPrefix = string.Empty;
            string time1 = time.Split(":")[0].Length.ToString() == "1" ? $"0{time}" : time;       // 1 digit la hour vantha athuku munnadi 0 add pannum
            string prefix = db.GenerateID("Prefix", ref makeupPrefix, "4");
            string query = "insert into tblmakeup values('" + prefix + "','" + name + "','" + phone + "','" + place + "','" + date + "', '" + time1 + "', '" + amount + "', '" + advance + "', '" + balance + "')";
            db.ExecuteCommand(query);
            db.UpdatePrefix(prefix, "MAC", "4");
            return "";
        }
        public IActionResult MakeUpDetails()
        {
            Makeup model = new Makeup();
            model.MakeUp = Common.GetMakeUpDetails(model, true);
            decimal grandtotal = 0;
            foreach (var item in model.MakeUp)
            {
                grandtotal += Convert.ToDecimal(item.MakeupAmount);
            }
            model.Total = Convert.ToString(grandtotal);
            return View(model);
        }
        [HttpPost]
        public IActionResult MakeUpDetails(Makeup model)
        {
            model.MakeUp = Common.GetMakeUpDetails(model, false);
            decimal grandtotal = 0;
            foreach (var item in model.MakeUp)
            {
                grandtotal += Convert.ToDecimal(item.MakeupAmount);
            }
            model.Total = Convert.ToString(grandtotal);
            return View(model);
        }
    }
}
