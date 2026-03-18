using Albat.Models;
using AlbatRoss.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;
using System.Xml.Linq;

namespace AlbatRoss.Models
{
    public class DataConnection
    {
        public static DataTable maptoolDt = new DataTable();
        public static DataTable toolmapdt = new DataTable();
        public static string ConnectionString { get; set; }
        public static void setConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }
        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public string GenerateID(string columnName, ref string columnPrefix, string companyId)
        {
            DataTable prefixData = GetPrefixDetails();

            string id = "";
            string idPrefix = "";
            if (prefixData.Rows.Count > 0)
            {
                if (companyId != "")
                {
                    List<DataRow> CompanyPrefixLst = (from DataRow dataRow in prefixData.Rows where Convert.ToString(dataRow["id"]) == companyId select dataRow).ToList();
                    idPrefix = Convert.ToString(CompanyPrefixLst[0][columnName]);
                }
                else
                {
                    idPrefix = Convert.ToString(prefixData.Rows[0][columnName]);
                }
            }
            int openBrIndex = idPrefix.IndexOf("[");
            int closeBrIndex = idPrefix.IndexOf("]");

            if ((openBrIndex >= 0 && closeBrIndex >= 0) && (closeBrIndex != openBrIndex + 1))
            {
                long maxId = Convert.ToInt64(idPrefix.Substring(openBrIndex + 1, closeBrIndex - openBrIndex - 1)) + 1;
                string[] openSplit = idPrefix.Split('[');
                string[] closeSplit = openSplit[1].Split(']');
                int length = Convert.ToInt32(closeSplit[0].Length);
                id = openSplit[0] + maxId.ToString("D" + length.ToString()) + closeSplit[1];
                columnPrefix = openSplit[0] + "[" + maxId.ToString("D" + length.ToString()) + "]" + closeSplit[1];
            }
            else
            {
                id = idPrefix;
                columnPrefix = idPrefix;
            }
            string prefixValue = id;

            return id;
        }
        public void UpdatePrefix(string prefixNo, string split, string id)
        {
            string prefixvalue = prefixNo.Split("~")[0];
            string mapprefix = prefixvalue.Split(split)[0];
            string mapprefixvalue = prefixvalue.Split(split)[1];
            string serviceCode = split + "[" + mapprefixvalue + "]";
            string query = "update tblprefixsettings set Prefix='" + serviceCode + "' where id='" + id + "'";
            ExecuteCommand(query);
        }
        public List<string> GetDb(ServiceModel models)
        {
            MySqlConnection connection = GetConnection();
            string ommand = "SELECT Productname,FORMAT(Rate,2)as Rate FROM allrate";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            List<String> list = new List<string>();
            while (Reader.Read())
                list.Add(Convert.ToString(Reader["id"]));
            list.Add(Convert.ToString(Reader["Productname"]));
            connection.Close();
            return list;
        }
        public string GetAllRate(string id)
        {
            List<string> stockList = new List<string>();
            string columns = string.Empty;
            MySqlConnection connection = GetConnection();
            string ommand = "select * from AllRate where Productname = '" + id.Split("~")[1] + "'";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            ServiceModel mode = new ServiceModel();
            while (Reader.Read())
                if (id.Split("~")[2] == "No")
                    columns = Convert.ToString(Reader["Productname"]) + "~" + Convert.ToString(Reader["Rate"]);
                else if (id.Split("~")[2] == "Yes")
                    columns = Convert.ToString(Reader["Productname"]) + "~" + Convert.ToString(Reader["MemberRate"]);
                 else
                    columns = Convert.ToString(Reader["Productname"]) + "~" + Convert.ToString(Reader["OfferRate"]);
            connection.Close();
            return columns;
        }
        public string ServiceSave1(ServiceModel values)
        {
            MySqlConnection connection = GetConnection();
            string ommand = string.Empty;
            int i = 0;
            ommand = "select ServiceNo from tblservice where ServiceNo = '" + values.ServiceCode + "' and ServiceName='" + values.Services + "'";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                i++;
            }
            connection.Close();
            if (i == 0)
            {
                DateTime timevalues = Convert.ToDateTime(values.Date);
                ommand = "insert into tblservice(ServiceNo,ServiceName,Qty,Price,Total,ClientName, Contact, ServiceDate, CurrenDate, Staffs) values('" + values.ServiceCode + "','" + values.Services + "','" + values.Quentity + "','" + values.Price + "','" + values.Total + "','" + values.Client + "','" + values.Contact + "','" + CommonUtil.ToMySQLDateTimeFormat(timevalues) + "','" + CommonUtil.ToMySQLDateTimeFormat(DateTime.Now) + "', '" + values.Staff + "')";
                MySqlCommand command1 = new MySqlCommand(ommand, connection);
                MySqlDataReader Reader1;
                connection.Open();
                command1.ExecuteNonQuery();
                connection.Close();
                return "Success";
            }
            else
            {
                DateTime timevalues = Convert.ToDateTime(values.Date);
                ommand = "update tblservice set Qty ='" + values.Quentity + "',total='" + values.Total + "',CurrenDate='" + CommonUtil.ToMySQLDateTimeFormat(DateTime.Now) + "',ClientName='" + values.Client + "',Contact='" + values.Contact + "',ServiceDate='" + CommonUtil.ToMySQLDateTimeFormat(timevalues) + "',Staff='" + values.Staff + "' where ServiceNo='" + values.ServiceCode + "' and ServiceName='" + values.Services + "'";
                MySqlCommand command1 = new MySqlCommand(ommand, connection);
                MySqlDataReader Reader1;
                connection.Open();
                command1.ExecuteNonQuery();
                connection.Close();
                return "Success";
            }
        }
        public string ServiceSave(string values)
        {
            MySqlConnection connection = GetConnection();
            string prefixvalue = values.Split("~")[0];
            string mapprefix = prefixvalue.Split("ALB")[0];
            string mapprefixvalue = prefixvalue.Split("ALB")[1];
            string serviceCode = "ALB" + "[" + mapprefixvalue + "]";
            string ommand1 = "update tblprefixsettings set Prefix='" + serviceCode + "' where id='1'";
            MySqlCommand updatecommand = new MySqlCommand(ommand1, connection);
            connection.Open();
            updatecommand.ExecuteNonQuery();
            connection.Close();
            return "Success";
        }
        public ClientModel GetClient(ClientModel models)
        {
            MySqlConnection connection = GetConnection();
            string ommand = "select ServiceName, Total, ClientName, Contact, CurrenDate from tblservice where contact ='" + models.Contact + "'";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            List<String> list = new List<string>();
            while (Reader.Read())
                list.Add(Convert.ToString(Reader["id"]));
            list.Add(Convert.ToString(Reader["Productname"]));
            connection.Close();
            return models;
        }
        public DataTable GetPrefixDetails()
        {
            List<string> stockList = new List<string>();
            string columns = string.Empty;
            DataTable table = new DataTable("Person");
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Prefix", typeof(string));

            MySqlConnection connection = GetConnection();
            string ommand = "select * from tblprefixsettings";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            DataTable maptoolDt = new DataTable();
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            ServiceModel mode = new ServiceModel();
            while (Reader.Read())
                table.Rows.Add(Convert.ToString(Reader["id"]), Convert.ToString(Reader["Prefix"]));
            connection.Close();
            return table;
        }
        public List<ClientModel> GetClientDetails(ClientModel models)
        {
            List<ClientModel> stockList = new List<ClientModel>();
            MySqlConnection connection = GetConnection();
            string ommand = "select  ServiceName, Total, ClientName, Contact, CurrenDate from tblservice where contact='" + models.Contact + "'";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            ClientModel mode = new ClientModel();
            while (Reader.Read())
            {
                stockList.Add(new ClientModel()
                {
                    ServiceName = Convert.ToString(Reader["ServiceName"]),
                    Amount = Convert.ToString(Reader["Total"]),
                    ClientName = Convert.ToString(Reader["ClientName"]),
                    Contact = Convert.ToString(Reader["Contact"]),
                    Date = Convert.ToDateTime(Reader["CurrenDate"])
                });
            }
            connection.Close();
            return stockList;
        }
        public List<DailyLogModel> DailyLogDetails(DailyLogModel models)
        {
            List<DailyLogModel> stockList = new List<DailyLogModel>();
            MySqlConnection connection = GetConnection();
            string ommand = string.Empty;
            if (models.Status == "true" || models.Status == null)
            {
                ommand = "select ClientName,Contact,ServiceName,Price,CurrenDate,ServiceDate,Qty,Total from tblservice where DATE_FORMAT(ServiceDate,'%Y-%m-%d')= DATE_FORMAT('" + CommonUtil.ToMySQLDateTimeFormat(DateTime.Now) + "', '%Y-%m-%d')";
            }
            else if (models.CheckBox)
            {
                ommand = "select ClientName,Contact,ServiceName,Price,CurrenDate,ServiceDate,Qty,Total from tblservice where Staffs ='" + models.Employee + "'";
            }
            else
            {
                ommand = "select ClientName,Contact,ServiceName,Price,CurrenDate,ServiceDate,Qty,Total from tblservice where (DATE_FORMAT(ServiceDate,'%Y-%m-%d') BETWEEN DATE_FORMAT('" + CommonUtil.ToMySQLDateTimeFormat(models.FromDate) + "', '%Y-%m-%d') and DATE_FORMAT('" + CommonUtil.ToMySQLDateTimeFormat(models.ToDate) + "', '%Y-%m-%d'));";
            }
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            int count = 1;
            while (Reader.Read())
            {
                stockList.Add(new DailyLogModel()
                {
                    Sno = Convert.ToString(count++),
                    ServiceName = Convert.ToString(Reader["ServiceName"]),
                    Amount = Convert.ToString(Reader["Price"]),
                    ClientName = Convert.ToString(Reader["ClientName"]),
                    Contact = Convert.ToString(Reader["Contact"]),
                    Date = Convert.ToDateTime(Reader["CurrenDate"]),
                    ServiceDate = Convert.ToDateTime(Reader["ServiceDate"]),
                    Qty = Convert.ToString(Reader["Qty"]),
                    Total = Convert.ToString(Reader["Total"]),
                });
            }
            connection.Close();
            return stockList;
        }
        public string SaveMemberShipData(string values)
        {
            MySqlConnection connection = GetConnection();

            DateTime join = Convert.ToDateTime(values.Split("~")[3]);
            DateTime finish = Convert.ToDateTime(values.Split("~")[4]);
            string ommand = "insert into tblMemberShip(MemberShipNo, Customername, Contact, JoinDate, FinishDate) values('" + values.Split("~")[0] + "'," +
                    "'" + values.Split("~")[1] + "','" + values.Split("~")[2] + "','" + CommonUtil.ToMySQLDateTimeFormat(join) + "','" + CommonUtil.ToMySQLDateTimeFormat(finish) + "')";
            MySqlCommand command = new MySqlCommand(ommand, connection);
            MySqlDataReader Reader;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "Success";
        }
        public List<MemberShipModel> GetmemberShipData(string id)
        {
            List<MemberShipModel> stockList = new List<MemberShipModel>();
            MySqlConnection connection = GetConnection();
            string command = string.Empty;
            if (id != null)
                command = "select * from tblmembership where contact='" + id.Split("~")[1] + "'";
            else
                command = "select * from tblmembership";
            MySqlCommand commands = new MySqlCommand(command, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = commands.ExecuteReader();
            while (Reader.Read())
            {
                stockList.Add(new MemberShipModel()
                {
                    MemberShipNo = Convert.ToString(Reader["MemberShipNo"]),
                    CustomerName = Convert.ToString(Reader["Customername"]),
                    Contact = Convert.ToString(Reader["Contact"]),
                    JoinDate = Convert.ToDateTime(Reader["JoinDate"]),
                    FinishDate = Convert.ToDateTime(Reader["FinishDate"])
                });
            }
            connection.Close();
            return stockList;
        }
        public string AddEmployee(string empid, string name, string phone, string user, string pass)
        {
            MySqlConnection connection = GetConnection();
            string query = "insert into userlogin(Empid,EmployeeName, UserName, Pwd, Phone, JoinDate) values('" + empid + "','" + name + "', '" + user + "', '" + pass + "', '" + phone + "', '" + CommonUtil.ToMySQLDateTimeFormat(DateTime.Now) + "')";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader Reader;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "Success";
        }
        public string EmployeeSearch(string user, string pass)
        {
            MySqlConnection connection = GetConnection();
            string data = "select * from userlogin where UserName='" + user + "' and pwd='" + pass + "'";
            MySqlCommand command = new MySqlCommand(data, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            int count = 0;
            while (Reader.Read())
            {
                count++;
            }
            connection.Close();
            return count == 0 ? "Faliue" : "Success";
        }
        public DataTable ReturnCommand(string query)
        {
            DataTable dt = new DataTable();
            MySqlConnection connection = GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            dt.Load(Reader);
            connection.Close();
            return dt;
        }
        public void ExecuteCommand(string query)
        {
            MySqlConnection connection = GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader Reader;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
