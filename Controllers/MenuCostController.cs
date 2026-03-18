using Albat.Models;
using AlbatRoss.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AlbatRoss.Controllers
{
    public class MenuCostController : Controller
    {
        DataConnection db = new DataConnection();
        public IActionResult Index()
        {
            Menu model = new Menu();
            model.ListValues = LoadModel();
            model.Items = CommonUtil.AllRate();
            return View(model);
        }
        public List<Menu> LoadModel()
        {
            string query = "select * from allrate";
            return db.ReturnCommand(query).AsEnumerable().Select(x => new Menu
            {
                Id = x["id"].ToString(),
                ProductName = x["Productname"].ToString(),
                Price = x["Rate"].ToString(),
                MemberRate = x["MemberRate"].ToString(),
                OfferRate = x["OfferRate"].ToString(),
            }).ToList();
        }
        [HttpPost]
        public IActionResult SaveDatas([FromBody] List<Menu> gridData)
        {
            if (gridData.Count > 0)
            {
                foreach (var item in gridData)
                {
                    var data = db.ReturnCommand("Select * from allrate where Productname = '" + item.ProductName + "'");
                    if (data.Rows.Count > 0)
                    {
                        db.ExecuteCommand("update allrate set Productname='" + item.ProductName + "', Rate='" + item.Price + "', MemberRate='" + item.MemberRate + "', OfferRate='" + item.OfferRate + "'  where id='" + item.Id + "'");
                    }
                }
            }
            return Json("");
        }
        public string AddServices(string serviceName, string price, string memberRate, string offerRate)
        {
            string result = "true";
            if (db.ReturnCommand("select Productname from allrate where Productname = '" + serviceName + "' ").Rows.Count > 0)
                result = "The Service was Already Exists";
            else                
                db.ExecuteCommand("insert into allrate(Productname, Rate, MemberRate, OfferRate) values('" + serviceName + "','" + price + "', '" + memberRate + "', '" + offerRate + "')");
            return result;
        }
        public string DeleteServices(string id, string serviceName)
        {
            if (db.ReturnCommand("select Productname from allrate where Id = '" + id + "' ").Rows.Count > 0)
                db.ExecuteCommand("delete from allrate where id= '" + id + "' ");
            return "Successfully Deleted for this Service Name  " + "- " + serviceName;
        }
    }
}
