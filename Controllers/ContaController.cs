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
    public class ContaController : ApiController
    {
        string connectionString = "Data Source=(LocalDB)\\MyInstance;AttachDbFilename=C:\\Users\\rafae\\source\\repos\\vCardPlatform\\App_Data\\Database.mdf;Integrated Security=True";
        //Co



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
                    string value = (string)reader["CreatedAt"];
                    if (value == null)
                    {
                        pedidoAInserir.CreatedAt = DateTime.Now.Ticks+"";
                    }
                    pedidoAInserir.CreatedAt = value;
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

        // GET: Conta/GetContaById/1
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

                Conta pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new Conta();
                    pedidoAInserir.Id = (int)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    string value = (string)reader["CreatedAt"];
                    if (value == null)
                    {
                        pedidoAInserir.CreatedAt = DateTime.Now.Ticks + "";
                    }
                    pedidoAInserir.CreatedAt = value;
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
