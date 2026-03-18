using AlbatRoss.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace Albat.Controllers
{
    public class UserController : Controller
    {
        DataConnection data = new DataConnection();
        public IActionResult Login()
        {
            return View();
        }
        public string AddUser(string name, string phone, string user, string pass)
        {
            if (data.ReturnCommand("select * from userlogin where UserName = '" + user + "'").Rows.Count == 0)
            {
                string macPrefix = string.Empty;
                string empid = data.GenerateID("Prefix", ref macPrefix, "3");
                data.AddEmployee(empid, name, phone, user, pass);
                string prefixNo = data.GenerateID("Prefix", ref macPrefix, "3");
                data.UpdatePrefix(prefixNo, "EMP", "3");
                return "Account Created Successfully";
            }
            else
                return "This Name Already Exists,You Have Change the User Name";
        }           
        public string LoginCheck(string user, string pass)
        {
            var msg = data.ReturnCommand("select * from userlogin where UserName = '" + user + "' and pwd = '" + pass + "'");
            var msg1 = data.ReturnCommand("select * from userlogin where UserName = '" + user + "' and pwd = '" + pass + "'");
            string LoginName = string.Empty;
            foreach (DataRow item in msg1.Rows)
            {
                LoginName = Convert.ToString(item["Empid"]);
            }
            HttpContext.Session.SetString("Login", LoginName);
            string value = HttpContext.Session.GetString("Login");
            if (msg.Rows.Count == 0)
                return "Invalid User Name!";
            else if (msg1.Rows.Count == 0)
                return "Invalid Password Please Check!";
            else
                return "Login Successfully";
        }
    }
}
