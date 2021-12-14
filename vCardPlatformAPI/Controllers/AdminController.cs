using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using vCardPlatformAPI.Models;

namespace vCardPlatformAPI.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        String[] Accounts = { };
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        AdminAccount account = null;
        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        MqttClient broker;
        String localhost = "127.0.0.1";

        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Admins WHERE Id=@id";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    account = new AdminAccount();
                    account.Id = (string)reader["Id"];
                    account.Email = (string)reader["Email"];
                    account.Password = (string)reader["Password"];
                    account.Nome = (string)reader["Nome"];
                    account.Enabled = (int)reader["Enabled"];
                  
                }

                reader.Close();
                connection.Close();
                if (account != null)
                {
                    return Ok(account);
                }
                else
                {
                    return NotFound();
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

        [Route("login")]
        [HttpGet]
        public IHttpActionResult GetByIdLogin()
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
                    adminConta.Enabled = (int)reader["Enabled"];  

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
        }                                       // Get Account By Email
        
        [Route("admins")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {

            SqlConnection connection = null;
            List<string> acc = new List<string>();
            

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();
                string cmdSQL = "SELECT * FROM Admins";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                SqlDataReader reader = command.ExecuteReader();

                AdminAccount adminConta = null;

                while (reader.Read())
                {
                    adminConta = new AdminAccount();
                    adminConta.Id = (string)reader["Id"];
                    adminConta.Nome = (string)reader["Nome"];
                    acc.Add("ID: "+adminConta.Id+" Nome: "+adminConta.Nome);

                }
                Accounts = acc.ToArray();
                reader.Close();
                connection.Close();
                if (Accounts != null)
                {
                    return Ok(Accounts);
                }
                else
                {
                    return Ok("Dead End");
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
        }                                             // Get All Accounts

        [Route("feed")]
        [HttpGet]
        public IHttpActionResult GetFeed()
        {

            SqlConnection connection = null;
            List<string> acc = new List<string>();


            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();
                string cmdSQL = "SELECT * FROM Movimentos";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    acc.Add((string)reader["Type"]);
                    acc.Add((string)reader["Id_Sender"]);
                    acc.Add((string)reader["Id_Receiver"]);
                    acc.Add((string)reader["Amount"]);

                }
                reader.Close();
                connection.Close();
                if (Accounts != null)
                {
                    MqttClient c = ConnectMosquitto();
                    PublishFeed(c,acc);
                    return Ok();
                }
                else
                {
                    return Ok("Dead End");
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

      

        [Route("status/{id}")] 
        [HttpPut]
        public IHttpActionResult PutStatus([FromBody]AdminAccount admin, String id)
        {

            AdminAccount current = null;
            // gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            
            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                current = admin;
                string cmdSQL = "UPDATE Admins set Enabled=@enabled WHERE Email=@email";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@email", current.Email);
                command.Parameters.AddWithValue("@enabled", current.Enabled);
               
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }
            int numRows = command.ExecuteNonQuery();

            connection.Close();

            if (numRows > 0)
            {
                return Ok();
            }

            return NotFound();
        }   // Change Account Status

        [Route("name/{id}")]
        [HttpPut]
        public IHttpActionResult PutName([FromBody] AdminAccount admin, String id)       // Change Account Name
        {

            AdminAccount current = null;
            // gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                current = admin;
                string cmdSQL = "UPDATE Admins set Nome=@nome WHERE Email=@email";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@email", current.Email);
                command.Parameters.AddWithValue("@nome", current.Nome);

            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }
            int numRows = command.ExecuteNonQuery();

            connection.Close();

            if (numRows > 0)
            {
                return Ok();
            }

            return NotFound();
        }
        
        [Route("password/{id}")] 
        [HttpPut]
        public IHttpActionResult PutPassword([FromBody] AdminAccount admin, String id)
        {

            AdminAccount current = null;
            // gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                current = admin;
                string cmdSQL = "UPDATE Admins set Password=@password WHERE Email=@email";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@email", current.Email);
                command.Parameters.AddWithValue("@password", current.Password);

            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }
            int numRows = command.ExecuteNonQuery();

            connection.Close();

            if (numRows > 0)
            {
                return Ok();
            }

            return NotFound();
        }// Change Account Password

        [Route("email/{id}")] 
        [HttpPut]
        public IHttpActionResult PutEmail([FromBody] AdminAccount admin, String id)
        {

            AdminAccount current = null;
            // gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                current = admin;
                string cmdSQL = "UPDATE Admins set Email=@email WHERE Id=@id";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@email", current.Email);
                command.Parameters.AddWithValue("@id", current.Id);

            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }
            int numRows = command.ExecuteNonQuery();

            connection.Close();

            if (numRows > 0)
            {
                return Ok();
            }

            return NotFound();
        }   // Change Account Email

        [Route("newadmin")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] AdminAccount user)
        {
            SqlConnection connection = null;
            SqlCommand command = null;


            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                if (user != null)
                {
                    string cmdSQL = "INSERT INTO Admins values(@Id,@Nome,@Email,@Enabled,@Password)";
                    command = new SqlCommand(cmdSQL, connection);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Nome", user.Nome);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Enabled", user.Enabled);

                }

                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok(user);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return BadRequest(e.Message + e.StackTrace);
            }

        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteAccount(String id)
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                string cmdSQL = "DELETE FROM Admins WHERE Id=@id";
                command = new SqlCommand(cmdSQL, connection);

                command.Parameters.AddWithValue("@id", id);

                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok();
                }

                return NotFound();
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

        private MqttClient ConnectMosquitto()
        {
            broker = new MqttClient(localhost);
            try
            {
                broker.Connect(Guid.NewGuid().ToString());
                if (!broker.IsConnected)
                {
                    Console.WriteLine("Error connecting to message broker...");
                    return null;
                }
                return broker;

            }
            catch (Exception)
            {
                Console.WriteLine("Error connecting to message broker...");
                return null;
            }
        }

        private void PublishFeed(MqttClient broker,List<string> acc)
        {
            char[] content;
            string total = "";
            foreach(String s in acc)
            {
                int i = 0;
                total = total + s+"\n";
                i++;
            }
            content = total.ToCharArray();
            broker.Publish("Feed", Encoding.UTF8.GetBytes(content));
            if (!broker.IsConnected)
            {
                Console.WriteLine("Error connecting to message broker...");
                return;
            }
            MessageBox.Show("Published to message broker");

        }
    }


}
