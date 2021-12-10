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
    [RoutePrefix("api/admin")]
    public class AdminImportartanteController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        AdminAccount account = null;

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
                string cmdSQL = "SELECT * FROM Admins WHERE Email=@Email";
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

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult PutStatus([FromBody] String id)
        {

            AdminAccount current = SearchAccount(id);

            SqlConnection connection = null;
            SqlCommand command = null;
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                if (current.Enabled == 1) { current.Enabled = 0; }
                else { current.Enabled = 1; }

                string cmdSQL = "UPDATE Admins set Enabled=@Enabled WHERE Email =@Email";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@Email", current.Email);
                command.Parameters.AddWithValue("@Enabled", current.Enabled);
                return Ok();
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
    
        private AdminAccount SearchAccount(String id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Admins WHERE Email=@Email";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@Email", id);
                SqlDataReader reader = command.ExecuteReader();

                AdminAccount current = null;

                while (reader.Read())
                {
                    current = new AdminAccount();
                    current.Id = (string)reader["Id"];
                    current.Email = (string)reader["Email"];
                    current.Password = (string)reader["Password"];
                    current.Nome = (string)reader["Nome"];
                    current.Enabled = (int)reader["Enabled"];

                }

                reader.Close();
                connection.Close();
               
                return current;
                
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    return null;
                }

                
            }
            return null;
        }
    
    }


}
