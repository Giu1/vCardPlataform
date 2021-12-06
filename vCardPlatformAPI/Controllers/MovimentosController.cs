using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatform.Models;
using vCardPlatformAPI.Models;

namespace vCardPlatformAPI.Controllers
{
    [RoutePrefix("api/movimento")]
    public class MovimentosController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("pagamento")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]MovimentoBancario movimento)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            Object obj = null;
            //check user_sender


            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT BankReference FROM Contas WHERE PhoneNumber=@idPedidosTable";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.IdSender);
                obj = command.ExecuteScalar()+"" ;

                if (int.Parse((string)obj) == 0)
                {
                    MessageBox.Show("IDSender not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("IDSender not found");
                return Ok(ex.Message);
            }
            movimento.BankRefSender = obj + "";
            //check user_reciever

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT BankReference FROM Contas WHERE PhoneNumber=@idPedidosTable";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.IdReceiver);
                 obj = command.ExecuteScalar();

                if (int.Parse((string)obj)==0)
                {
                    MessageBox.Show("IDSender not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("IDReciever not found");
                return Ok(ex.Message);
            }
            movimento.BankRefReceiver = obj + "";
            movimento.Id = DateTime.Now.Ticks + "";
            movimento.Date = DateTime.Now.ToString();
            MovimentoBancario MCredito = movimento;
            MCredito.Type = TypeOfMoviment.Credito + "";
            MovimentoBancario MDebito = movimento;
            MDebito.Type = TypeOfMoviment.Debito + "";
            MDebito.Amount = movimento.Amount * -1;

            //emitir credito

            string link = String.Format("https://localhost:44360/bank_1/movimentos");

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(MCredito);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Erro ao emitir nota");
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota");

                return Ok(ex.Message);
            }


            //emitir debito

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(MDebito);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Erro ao emitir nota");
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota");

                return Ok(ex.Message);
            }

            
            return Ok("Notas de credito inseridas");
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