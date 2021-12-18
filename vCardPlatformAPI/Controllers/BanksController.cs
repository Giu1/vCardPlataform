using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using vCardPlatform.Models;

namespace vCardPlatformAPI.Controllers
{
    [RoutePrefix("api/bank")]
    public class BanksController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Banks";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                SqlDataReader reader = command.ExecuteReader();

                Bank pedidoAInserir = null;
                List<Bank> lista = new List<Bank>();
                while (reader.Read())
                {
                    pedidoAInserir = new Bank();
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.Date = (string)reader["Date"];
                    pedidoAInserir.Name = (string)reader["Name"];

                    lista.Add(pedidoAInserir);
                }

                reader.Close();
                connection.Close();
                return Ok(lista);
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
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Banks WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                Bank pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new Bank();
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.Date = (string)reader["Date"];
                    pedidoAInserir.Name = (string)reader["Name"];


                }

                reader.Close();
                connection.Close();
                if (pedidoAInserir != null)
                {
                    return Ok(pedidoAInserir);
                }
                else
                {
                    return Ok("Erro - Id nao encontrado");
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
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] Bank value)
        {
            SqlConnection connection = null;
            string cmdSQL = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                cmdSQL = "INSERT INTO Banks VALUES (@a,@b,@c)";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@a", value.Id);
                command.Parameters.AddWithValue("@b", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@c", value.Name);
                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok("Sucesso");
                }

                return Ok("Erro - Id nao encontrado");

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

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}