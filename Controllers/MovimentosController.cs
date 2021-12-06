using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using vCardPlatform.Models;

namespace vCardPlatform.Controllers
{
    [RoutePrefix("bank_1/movimentos")]
    public class MovimentosController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;

        [Route("")]
        [HttpPost]
        public IHttpActionResult RegistarMovimentos([FromBody] MovimentoBancario movimento)
        {
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            connection.Open();
            string cmdSQL = "SELECT COUNT(*) FROM movimentos";
            SqlCommand command = new SqlCommand(cmdSQL, connection);
            int reader = (int)command.ExecuteScalar();

            try
            {
    
                cmdSQL = "INSERT INTO Movimentos VALUES (@z,@a,@b,@c,@d,@e,@f,@g)";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@z", reader);
                command.Parameters.AddWithValue("@a", movimento.IdSender);
                command.Parameters.AddWithValue("@b", movimento.BankRefSender);
                command.Parameters.AddWithValue("@c", movimento.IdReceiver);
                command.Parameters.AddWithValue("@d", movimento.BankRefReceiver);
                command.Parameters.AddWithValue("@e", movimento.Amount);
                command.Parameters.Add(new SqlParameter("@f", string.IsNullOrEmpty(movimento.Description) ? (object)DBNull.Value : movimento.Description));
                
                command.Parameters.AddWithValue("@g", DateTime.Now.Ticks +"");

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
        [HttpGet]
        public IHttpActionResult GetMovimento(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Movimentos WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                MovimentoBancario pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new MovimentoBancario();
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.IdSender = (string)reader["Id_Sender"];
                    pedidoAInserir.BankRefSender = (string)reader["Id_Bank_Sender"];
                    pedidoAInserir.IdReceiver = (string)reader["Id_Receiver"];
                    pedidoAInserir.BankRefReceiver = (string)reader["Id_Bank_Receiver"];
                    pedidoAInserir.Amount = (float)reader["Amount"];
                    pedidoAInserir.Date = (string)reader["Date"];
                    pedidoAInserir.Description = (string)reader["Description"];

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
    }
}
