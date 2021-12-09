using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vCardPlatformAPI.Models;

namespace vCardPlatformAPI.Controllers
{
    [RoutePrefix("api/teste")]
    public class AdminImportartanteController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;


        [Route("")]
        [HttpGet]
        public IHttpActionResult GetById()
        {

            var re = Request;
            var headers = re.Headers;
            string token = null;
            if (headers.Contains("Custom"))
            {
                token = headers.GetValues("Custom").First();
            }
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();
                string cmdSQL = "SELECT * FROM Admin WHERE Email=@Email";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@Email", token);
                SqlDataReader reader = command.ExecuteReader();

                AdminAccount adminConta = null;

                while (reader.Read())
                {
                    adminConta = new AdminAccount();
                    adminConta.Id = (string)reader["Id"];
                    adminConta.Email = (string)reader["Email"];
                    adminConta.Password = (string)reader["Password"];
                    adminConta.Nome = (string)reader["Nome"];

                }

                reader.Close();
                connection.Close();
                if (adminConta != null)
                {
                    return Ok(adminConta);
                }
                else
                {
                    return Ok("It works");
                }
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }
        }
    }
}
