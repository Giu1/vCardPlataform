using System;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using vCardPlatform.Models;

namespace vCardPlatform.Controllers
{
   [RoutePrefix("api/conta")]
    public class ContaController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        // GET: Conta/GetContaById/1
        public IHttpActionResult GetContaById(int id)
        {
            SqlConnection connection = null;
             try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                Conta pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new Conta();
                    pedidoAInserir.Id = (int)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    pedidoAInserir.Email = (string)reader["Email"];
                    pedidoAInserir.PhoneNumber = (string)reader["PhoneNumber"];
                    pedidoAInserir.ConfirmationCode = (int)reader["ConfirmationCode"];
                    
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

                return Ok(e.Message+e.StackTrace);
            }
        }

        [Route("")]
        public IHttpActionResult GetAllContas()
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                
                SqlDataReader reader = command.ExecuteReader();
                LinkedList<Conta> contas = new LinkedList<Conta>(); 

                

                while (reader.Read())
                {
                    Conta pedidoAInserir = new Conta();
                    pedidoAInserir.Id = (int)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    pedidoAInserir.Email = (string)reader["Email"];
                    pedidoAInserir.PhoneNumber = (string)reader["PhoneNumber"];
                    pedidoAInserir.ConfirmationCode = (int)reader["ConfirmationCode"];
                    contas.AddLast(pedidoAInserir);
                }

                reader.Close();
                connection.Close();
                if (contas.Count != 0)
                {
                    return Ok(contas);
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

        
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostConta([FromBody] Conta value)
        {
            SqlConnection connection = null;
            
            try
            {
               
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "INSERT INTO Contas VALUES (@z,@a,@b,@c,@d,@e,@f)";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@z", value.Id);
                command.Parameters.AddWithValue("@a", value.AccountOwner);
                command.Parameters.AddWithValue("@b", value.Balance);
                command.Parameters.AddWithValue("@c", DateTime.Now.Ticks+"");
                command.Parameters.AddWithValue("@d", value.Email);
                command.Parameters.AddWithValue("@e", value.ConfirmationCode);
                command.Parameters.AddWithValue("@f", value.PhoneNumber);

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

                return Ok(e.Message + e.StackTrace); ;
            }

        }


        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteConta(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "DELETE FROM contas WHERE Id=@IdPedido";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@IdPedido", id);
                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok();
                }

                return NotFound();

            }
            catch (Exception)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return NotFound();
            }
        }


        [Route("{int:id}")]
        [HttpPut]
        public IHttpActionResult PutConta(int id,[FromBody] Conta value)
        {
            SqlConnection connection = null;

            try
            {
               


                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                cmdSQL = "INSERT INTO Contas(AccountOwner,Email,ConfirmationCode,PhoneNumber) VALUES (@a,@b,@c,@d)";
                command = new SqlCommand(cmdSQL, connection);
                if(value.AccountOwner != null)
                {
                    command.Parameters.AddWithValue("@a", value.AccountOwner);
                }
                            
                if(value.Email != null && IsValidEmail(value.Email))
                {
                    command.Parameters.AddWithValue("@b", value.Email);
                }

                if (value.ConfirmationCode> 9999 && value.ConfirmationCode<1000)
                {
                    command.Parameters.AddWithValue("@c", value.ConfirmationCode);
                }

                if (value.PhoneNumber.Length>8 && value.PhoneNumber.Length<14 )
                {
                    command.Parameters.AddWithValue("@d", value.PhoneNumber);
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

                return Ok(e.Message + e.StackTrace); ;
            }

        }


        //https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}
