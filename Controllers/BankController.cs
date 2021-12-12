using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace vCardPlatform.Controllers
{
    [RoutePrefix("bank_1/bank")]
    public class BankController : ApiController
    {
        string[] BankValues = { };

        private string bankId = "1111";
        // GET: Bank
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;
        
        
        [Route("")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            SqlConnection connection = null;
            List<string> acc = new List<string>();
            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();
                string cmdSQL = "SELECT * FROM BankValues";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                SqlDataReader reader = command.ExecuteReader();

                string credito = "";
                string debito = "";

                while (reader.Read())
                {
                    credito = (string)reader["CreditoMax"];
                    debito = (string)reader["DebitoMax"];
                    acc.Add(bankId);
                    acc.Add(credito);
                    acc.Add(debito);

                }
                BankValues = acc.ToArray();
                reader.Close();
                connection.Close();
                if (acc != null)
                {
                    return Ok(BankValues);
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


        [Route("update")]
        [HttpPut]
        public IHttpActionResult PutValues([FromBody]String[] list)       // Change Account Name
        {

            String[] values = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                values = list;
                string cmdSQL = "UPDATE BankValues set CreditoMax=@credito,DebitoMax=@debito WHERE refId=@id";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@credito",list[1].Trim());
                command.Parameters.AddWithValue("@debito", list[2].Trim());
                command.Parameters.AddWithValue("@id", list[0].Trim());

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
    }
}