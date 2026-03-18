using Albat.Models;
using AlbatRoss.Models;
using DocumentFormat.OpenXml.EMMA;
using System.Data;

namespace AlbatRoss.Controllers
{
    public class Common
    {
        static DataConnection db = new DataConnection();
        public static List<Makeup> GetMakeUpDetails(Makeup model, bool status)
        {
            string query = string.Empty;
            if (status)
                query = "select * from tblmakeup where date(makeupdate) = '" + CommonUtil.ToMySQLDateFormat(DateTime.Now) + "'";
            else
                query = "select * from tblmakeup where Date(makeupDate) BETWEEN '" + CommonUtil.ToMySQLDateFormat(model.FromDate) + "' and '" + CommonUtil.ToMySQLDateFormat(model.ToDate) + "'";

            return db.ReturnCommand(query).AsEnumerable().Select(x => new Makeup
            {
                Name = x["Name"].ToString(),
                Phone = x["phone"].ToString(),
                Place = x["Place"].ToString(),
                MakeupDate = Convert.ToDateTime(x["makeupdate"]).ToString("dd-MM-yyyy"),
                Time = x["Time"].ToString(),
                MakeupAmount = x["Amount"].ToString(),
                Amount = x["Balance"].ToString(),
                Total = x["Advance"].ToString(),
            }).ToList();
        }
    }
}
