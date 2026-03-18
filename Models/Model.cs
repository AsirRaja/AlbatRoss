using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Albat.Models
{
    public class ServiceModel
    {
        public List<SelectListItem> Service { get; set; }
        public string Services { get; set; }
        public string AvailableValues { get; set; }
        public int SelectedValue { get; set; }
        public List<SelectListItem> Items { get; set; }
        public List<SelectListItem> Employee { get; set; }
        public List<ServiceModel> stockgrid { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string ServiceCode { get; set; }
        public string MemberShip { get; set; }
        public string Total { get; set; }
        public string Quentity { get; set; }
        public string Date { get; set; }
        public string BillofDate { get; set; }
        public string Contact { get; set; }
        public string Client { get; set; }
        public string Staff { get; set; }
    }
    public class ClientModel
    {
        public string ServiceName { get; set; }
        public string ClientName { get; set; }
        public string Amount { get; set; }
        public string Contact { get; set; }
        public DateTime Date { get; set; }
        public List<ClientModel> ClientDetails { get; set; }
    }
    public class DailyLogModel
    {
        public string ServiceName { get; set; }
        public string ClientName { get; set; }
        public string Amount { get; set; }
        public string Contact { get; set; }
        public DateTime Date { get; set; }
        public List<DailyLogModel> DailyLog { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
        public string Sno { get; set; }
        public string GrandTotal { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public bool CheckBox { get; set; }
        public string Employee { get; set; }
    }
    public class MemberShipModel
    {
        public string MemberShipNo { get; set; }
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FinishDate { get; set; }
        public List<MemberShipModel> MemberList { get; set; }
    }
    public class MonthlyCost
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public string Total { get; set; }
        public List<MonthlyCost> stock { get; set; }
        public string Services { get; set; }
    }
    public class Menu
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string MemberRate { get; set; }
        public string OfferRate { get; set; }
        public List<Menu> ListValues { get; set; } = new List<Menu>();
        public List<SelectListItem> Items { get; set; }
    }
    public class Makeup
    {
        public List<Makeup> MakeUp { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Place { get; set; }
        public string MakeupAmount { get; set; }
        public string MakeupDate { get; set; }
        public string Time { get; set; }
        public string Total { get; set; }
        public string Amount { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
    }
}
