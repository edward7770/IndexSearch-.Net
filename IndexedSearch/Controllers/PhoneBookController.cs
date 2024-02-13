using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndexedSearch.Controllers
{
    public class PhoneBookController : Controller
    {
        private Models.SearchModel SearchModel = new Models.SearchModel();
        public ActionResult Index()
        {
            return View();
        }
        string SqlGetConnectionString(string ConfigPath, string ConnectionStringName)
        {
            System.Configuration.Configuration rootWebConfig =
                            System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(ConfigPath);
            System.Configuration.ConnectionStringSettings connectionString =
                rootWebConfig.ConnectionStrings.ConnectionStrings[ConnectionStringName];
            if (connectionString == null || string.IsNullOrEmpty(connectionString.ConnectionString))
                throw new Exception("Fatal error: Connection string is missing from web.config file");

            return connectionString.ConnectionString;
        }
        public ActionResult Search(string text)
        {
            using (SqlConnection connection =
                       new SqlConnection(this.SqlGetConnectionString("/Web.Config", "PhoneBookDB")))
            {
                try
                {
                    string SqlQuery = @"SELECT dbo.Contacts.* FROM dbo.Contacts";
                    if (Request.IsAjaxRequest() && text != "")
                        SqlQuery += " WHERE dbo.Contacts.ContactName LIKE @text OR dbo.Contacts.Phone LIKE @text";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@text", String.Format("%{0}%", text));

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read() && reader.HasRows != false)
                        {
                            Models.PhoneBookEntry PhoneEntry = new Models.PhoneBookEntry();
                            PhoneEntry.ContactID = Int32.Parse(reader["ContactID"].ToString());
                            PhoneEntry.ContactName = reader["ContactName"].ToString();
                            PhoneEntry.Phone = reader["Phone"].ToString();

                            if ((!PhoneEntry.ContactID.Equals("")) &&
                                (!PhoneEntry.ContactName.Equals("")) && (!PhoneEntry.Phone.Equals("")))
                                SearchModel.PhoneList.Add(PhoneEntry);
                        }

                        reader.Close();
                    }
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            return PartialView(SearchModel.PhoneList);
        }
        public ActionResult Create(string person, string phone)
        {
            using (SqlConnection connection =
                       new SqlConnection(this.SqlGetConnectionString("/Web.Config", "PhoneBookDB")))
            {
                try
                {
                    string SqlQuery = @"INSERT INTO dbo.Contacts VALUES (@person, @phone)";
                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@person", person);
                    command.Parameters.AddWithValue("@phone", phone);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }


            return RedirectToAction("Index");
        }
    }
}