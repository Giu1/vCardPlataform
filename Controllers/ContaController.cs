using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using vCardPlatform.Models;

namespace vCardPlatform.Controllers
{
    [RoutePrefix("bank_1/conta")]
    public class ContaController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        // GET: Conta/GetContaById/1
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetConta(string id)
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
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    pedidoAInserir.Email = (string)reader["Email"];
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

                return Ok(e.Message + e.StackTrace);
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
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    pedidoAInserir.Email = (string)reader["Email"];
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

            string cmdSQL = null;
            SqlCommand command = null;
            


            try
            {

                connection = new SqlConnection(connectionString);
                connection.Open();
                cmdSQL = "INSERT INTO Contas VALUES (@z,@a,@b,@c,@d,@e,@f)";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@z", DateTime.Now.Ticks +"");
                command.Parameters.AddWithValue("@a", value.AccountOwner);
                command.Parameters.AddWithValue("@b", value.Balance);
                command.Parameters.AddWithValue("@c", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@d", value.Email);
                command.Parameters.AddWithValue("@e", value.ConfirmationCode);

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
        public IHttpActionResult DeleteConta(string id)
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


        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult PutConta(string id, [FromBody] Conta value)
        {
            SqlConnection connection = null;

            try
            {
                //busco elemento antigo
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                Conta contaOld = null;

                while (reader.Read())
                {
                    contaOld = new Conta();
                    contaOld.Id = (string)reader["Id"];
                    contaOld.Balance = (float)reader["Balance"];
                    contaOld.AccountOwner = (string)reader["AccountOwner"];
                    contaOld.CreatedAt = (string)reader["CreatedAt"];
                    contaOld.Email = (string)reader["Email"];
                    contaOld.ConfirmationCode = (int)reader["ConfirmationCode"];

                }

                reader.Close();
                
                // comparo com o elemento enviado pelo request
                cmdSQL = "UPDATE Contas set AccountOwner=@AccountOwner,Email=@Email,Balance=@balance,ConfirmationCode=@ConfirmationCode WHERE id =@id";
                command = new SqlCommand(cmdSQL, connection);
                
                if (value.AccountOwner != null)
                {
                    command.Parameters.AddWithValue("@AccountOwner", value.AccountOwner);
                }
                else
                {
                    command.Parameters.AddWithValue("@AccountOwner", contaOld.AccountOwner);
                }

                if (value.Balance != float.MinValue)
                {
                    command.Parameters.AddWithValue("@balance", value.Balance);
                }
                else
                {
                    command.Parameters.AddWithValue("@balance", contaOld.Balance);
                }

                if (value.Email != null && IsValidEmail(value.Email))
                {
                    command.Parameters.AddWithValue("@Email", value.Email);
                }
                else
                {
                    command.Parameters.AddWithValue("@Email", contaOld.Email);
                }

                if (value.ConfirmationCode >= 1000 && value.ConfirmationCode <= 9999)
                {
                    command.Parameters.AddWithValue("@ConfirmationCode", value.ConfirmationCode);
                }
                else
                {
                    command.Parameters.AddWithValue("@ConfirmationCode", contaOld.ConfirmationCode);
                }

               



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
