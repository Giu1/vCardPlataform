using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

using vCardPlatformAPI.Models;

namespace vCardPlatformApi.Controllers
{
    [RoutePrefix("api/adminconsole")]

    public class AdminController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        // GET: Conta
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetById(String Email)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                
                connection.Open();
                string cmdSQL = "SELECT * FROM Admin WHERE Email=@Email";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@Email", Email);
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





        [Route("")]
        [HttpGet]
        public String Get(String Email) => "1";




        [Route("admin/tuta")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Admin ";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                SqlDataReader reader = command.ExecuteReader();

                LinkedList<AdminAccount> admins = new LinkedList<AdminAccount>();
               

                while (reader.Read())
                {
                     AdminAccount loginAdmin = null;
                    loginAdmin = new AdminAccount();
                    loginAdmin.Id = (string)reader["Id"];
                    loginAdmin.Email = (string)reader["Email"];
                    loginAdmin.Password = (string)reader["Password"];
                    loginAdmin.Nome = (string)reader["Nome"];
                    admins.AddLast(loginAdmin);

                }


                reader.Close();
                connection.Close();
                return Ok(admins);
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
        public IHttpActionResult Put([FromBody] Conta user,int id)
        {
            //gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            Conta oldUser = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE PhoneNumber=@idPedidosTable";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                reader = command.ExecuteReader();

                

                while (reader.Read())
                {
                    oldUser = new Conta();
                    oldUser.AccountOwner = (string)reader["AccountOwner"];
                    oldUser.Email = (string)reader["Email"];
                    oldUser.ConfirmationCode = (int)reader["ConfirmationCode"];
                    oldUser.Password = (string)reader["Password"];
                    oldUser.Balance = (float)reader["Balance"];
                    if (reader["Photo"] != DBNull.Value)
                    {
                        oldUser.Photo = Convert.ToBase64String((Byte[])reader["Photo"]);
                    }
                    else
                    {
                        oldUser.Photo = null;
                    }


                }

                reader.Close();
                connection.Close();
                if (oldUser == null)
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


            //inserts full client


            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                


                if (user.Photo != null)
                {
                    string cmdSQL = "UPDATE Contas set AccountOwner=@accountowner,ConfirmationCode=@confirmationcode,Password=@password,Balance=@balance,Email=@email,Photo =@photo WHERE PhoneNumber =@id";
                    command = new SqlCommand(cmdSQL, connection);
                    command.Parameters.AddWithValue("@photo", Convert.FromBase64String(user.Photo));

                }
                else
                {
                    string cmdSQL = "UPDATE Contas set AccountOwner=@accountowner,ConfirmationCode=@confirmationcode,Balance=@balance,Password=@password,Email=@email WHERE PhoneNumber =@id";
                    command = new SqlCommand(cmdSQL, connection);
                }

                
                command.Parameters.AddWithValue("@id", id);

                if (user.Balance != 0)
                {
                    command.Parameters.AddWithValue("@balance", user.Balance);
                }
                else
                {
                    command.Parameters.AddWithValue("@balance", oldUser.Balance);
                }

                if (user.AccountOwner != null)
                {
                    command.Parameters.AddWithValue("@accountowner", user.AccountOwner);
                }
                else
                {
                    command.Parameters.AddWithValue("@accountowner", oldUser.AccountOwner);
                }

                if ( user.ConfirmationCode!=0)
                {
                    if (user.ConfirmationCode>=1000 && user.ConfirmationCode<=9999)
                    {
                        command.Parameters.AddWithValue("@confirmationcode", user.ConfirmationCode);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@confirmationcode", oldUser.ConfirmationCode);
                        return Ok("Confirmation code não tem formato valido");
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@confirmationcode", oldUser.ConfirmationCode);
                }

                

                if (user.Password!=null)
                {
                    command.Parameters.AddWithValue("@password", user.Password);
                }
                else
                {
                    command.Parameters.AddWithValue("@password", oldUser.Password);
                }

                if (user.Email!=null)
                {
                    command.Parameters.AddWithValue("@email", user.Email);
                }
                else
                {
                    command.Parameters.AddWithValue("@email", oldUser.Email);
                }
                       

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

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(Conta user)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                if (user.Photo != null)
                {
                    string cmdSQL = "INSERT INTO Contas values(@PhoneNumber,@accountowner,0,@CreatedAt,@email,@confirmationcode,@photo,@password)";
                    command = new SqlCommand(cmdSQL, connection);
                    command.Parameters.AddWithValue("@photo", Convert.FromBase64String(user.Photo));
                }
                else
                {
                    string cmdSQL = "INSERT INTO Contas values(@PhoneNumber,@accountowner,0,@CreatedAt,@email,@confirmationcode,NULL,@password)";
                    command = new SqlCommand(cmdSQL, connection);
                }

                command.Parameters.AddWithValue("@PhoneNumber", user.Id);
                command.Parameters.AddWithValue("@accountowner", user.AccountOwner);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("dd / MM / yyyy"));
                command.Parameters.AddWithValue("@confirmationcode", user.ConfirmationCode);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@email", user.Email);

                //todo
                /*
                 * pedidoAInserir.BankId = (string)reader["BankId"];
                    pedidoAInserir.BankRef = (string)reader["BankReference"];
                 */

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
    }
}