using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using vCardPlatform.Models;

namespace vCardPlatformApi.Controllers
{
    [RoutePrefix("api/conta")]
    public class ContaController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        // GET: Conta
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE PhoneNumber=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                User pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new User();
                    pedidoAInserir.Id = (int)reader["PhoneNumber"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    pedidoAInserir.Email = (string)reader["Email"];
                    pedidoAInserir.ConfirmationCode = (int)reader["ConfirmationCode"];
                    pedidoAInserir.Password = (string)reader["Password"];
                    if (reader["Photo"] != DBNull.Value)
                    {
                        byte[] data = (byte[])reader["Photo"];
                        string result = Convert.ToBase64String(data);
                        pedidoAInserir.Photo = result;

                    }
                    else
                    {
                        pedidoAInserir.Photo = null;
                    }


                }

                reader.Close();
                connection.Close();
                if (pedidoAInserir != null)
                {
                    return Ok(pedidoAInserir);
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








        [Route("banks/{id}")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            string link = String.Format("https://localhost:44360/bank_1/bank");

            WebRequest request = WebRequest.Create(link);
            request.Method = "GET";
            HttpWebResponse response = null;

            response = (HttpWebResponse)request.GetResponse();

            string strResul = null;

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                strResul = reader.ReadToEnd();
                reader.Close();
            }



            

            return Ok();
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Put([FromBody] User user,int id)
        {
            //gets the user in BD

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            User oldUser = null;
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
                    oldUser = new User();
                    oldUser.AccountOwner = (string)reader["AccountOwner"];
                    oldUser.Email = (string)reader["Email"];
                    oldUser.ConfirmationCode = (int)reader["ConfirmationCode"];
                    oldUser.Password = (string)reader["Password"];
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
                    string cmdSQL = "UPDATE Contas set AccountOwner=@accountowner,ConfirmationCode=@confirmationcode,Password=@password,Email=@email,Photo =@photo WHERE PhoneNumber =@id";
                    command = new SqlCommand(cmdSQL, connection);
                    command.Parameters.AddWithValue("@photo", Convert.FromBase64String(user.Photo));

                }
                else
                {
                    string cmdSQL = "UPDATE Contas set AccountOwner=@accountowner,ConfirmationCode=@confirmationcode,Password=@password,Email=@email WHERE PhoneNumber =@id";
                    command = new SqlCommand(cmdSQL, connection);
                }

                
                command.Parameters.AddWithValue("@id", id);

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
        public IHttpActionResult Post(User user)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

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