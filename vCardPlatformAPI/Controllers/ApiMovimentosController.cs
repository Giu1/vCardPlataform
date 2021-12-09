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


using User = vCardPlatformAPI.Models.User;
using ContaBankSide = vCardPlatform.Models.Conta;
using vCardPlatform.Models;

namespace vCardPlatformAPI.Controllers
{
    [RoutePrefix("api/movimentos")]
    public class ApiMovimentosController : ApiController
    {

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString;

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("")]
        [HttpPost]
        public IHttpActionResult RegistarMovimentos([FromBody] MovimentoBancario movimento)
        {
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            connection.Open();
            string cmdSQL = null;
            SqlCommand command = new SqlCommand(cmdSQL, connection);


            try
            {

                cmdSQL = "INSERT INTO Movimentos VALUES (@z,@a,@b,@c,@d,@e,@f,@g,@h)";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@z", DateTime.Now.Ticks + "");
                command.Parameters.AddWithValue("@a", movimento.IdSender);
                command.Parameters.AddWithValue("@b", movimento.BankRefSender);
                command.Parameters.AddWithValue("@c", movimento.IdReceiver);
                command.Parameters.AddWithValue("@d", movimento.BankRefReceiver);

                if (movimento.Amount > 0)
                {
                    command.Parameters.AddWithValue("@e", movimento.Amount);
                }
                else
                {
                    return NotFound();
                }
                command.Parameters.Add(new SqlParameter("@f", string.IsNullOrEmpty(movimento.Description) ? (object)DBNull.Value : movimento.Description));
                string aqui = movimento.Type.ToString();
                command.Parameters.AddWithValue("@g", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@h", movimento.Type.ToString());

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

        [Route("paga")]
        [HttpPost]
        public string Pagamento()
        {
            return "value";
        }

        [Route("levantar")]
        [HttpPost]
        public string Levantar()
        {
            return "value";
        }
        [Route("depositar")]
        [HttpPost]
        public IHttpActionResult Depositar([FromBody]MovimentoBancario movimentoBancario)
        {
            movimentoBancario.IdReceiver = "1111111111111"; //referencia para conta do banco da plataforma
            movimentoBancario.BankRefReceiver = "1111";

            string link = String.Format("http://localhost:50766/api/movimentos/trans");

            //emite transferencia entre utilizador e banco

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimentoBancario);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                

            }
            catch (Exception ex)
            {
                
                return Ok("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace);
            }
            

            //atualiza saldo na api
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                
                    string cmdSQL = "UPDATE Contas set Balance=Balance + @amount WHERE BankID = @id";
                    command = new SqlCommand(cmdSQL, connection);
                    command.Parameters.AddWithValue("@id", movimentoBancario.IdSender);

                command.Parameters.AddWithValue("@amount", movimentoBancario.Amount);


                if (!(command.ExecuteNonQuery() > 0))
                {
                    connection.Close();
                    return BadRequest("Erro ao atualizar saldo");
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

            //regista movimento 

            //emitir movimentos sender


            
            
            
            movimentoBancario.Type = TypeOfMoviment.Debito;
            

            link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimentoBancario);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {


                    return Ok("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {


                return Ok("Erro ao emitir nota - emitir movimento Api side " + ex.Message + "\n" + ex.StackTrace);
            }

            return Ok("Depósito realizado com sucesso");
        }



        [Route("trans")]

        [HttpPost]
        public IHttpActionResult Transferencia([FromBody] MovimentoBancario movimento)
        {

            SqlConnection connection = null;
            SqlCommand command = null;
            string link = null;
            ContaBankSide contaSender = null;
            ContaBankSide contaReciever = null;
            string readerBankSender = null;
            string readerBankReceiver = null;

            //TODO check other variables
            //check user_sender

            //get bank

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT NAME FROM Banks WHERE ID=@idPedidosTable";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.BankRefSender);

                readerBankSender = (string) command.ExecuteScalar();

                if (readerBankSender == null)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return Ok("Erro BnkRefSender inválido");
                }
            }
            catch (Exception ex)
            {

                return Ok("IDSender not found" + ex.Message);
            }

            //get conta sender
            string linkSave = String.Format("https://localhost:44360/"+ readerBankSender + "/conta/"+movimento.IdSender);
            try
            {
                WebRequest requestPassword = WebRequest.Create(linkSave);
                requestPassword.Method = "GET";
                HttpWebResponse responsePassword = null;

                responsePassword = (HttpWebResponse)requestPassword.GetResponse();

                String strResul = null;
                using (Stream stream = responsePassword.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(stream);
                    strResul = reader.ReadToEnd();
                    reader.Close();
                }

                var serializer = new JavaScriptSerializer();

                contaSender = (ContaBankSide)serializer.Deserialize(strResul, typeof(ContaBankSide));
            }
            catch (Exception ex)
            {

                return Ok("IDSender not found" + ex.Message);
            }

            //check user_receiver

            //get bank

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT NAME FROM Banks WHERE ID=@idPedidosTable";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.BankRefReceiver);

                readerBankReceiver = (string)command.ExecuteScalar();

                if (readerBankReceiver == null)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return Ok("Erro BnkRefSender inválido");
                }
            }
            catch (Exception ex)
            {

                return Ok("IDSender not found" + ex.Message);
            }

            //get conta receiver

            link = String.Format("https://localhost:44360/" + readerBankReceiver + "/conta/" + movimento.IdReceiver);
            try
            {
                WebRequest requestPassword = WebRequest.Create(link);
                requestPassword.Method = "GET";
                HttpWebResponse responsePassword = null;

                responsePassword = (HttpWebResponse)requestPassword.GetResponse();

                String strResul = null;
                using (Stream stream = responsePassword.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(stream);
                    strResul = reader.ReadToEnd();
                    reader.Close();
                }

                var serializer = new JavaScriptSerializer();

                contaReciever = (ContaBankSide)serializer.Deserialize(strResul, typeof(ContaBankSide));
            }
            catch (Exception ex)
            {


                return Ok("IDSender not found" + ex.Message);
            }




            //alterar saldos

            //update no balanço da conta do sender

            link = String.Format("https://localhost:44360/bank_1/conta/" + contaSender.Id);

            ContaBankSide obj = new ContaBankSide();
            obj.Id = contaSender.Id;
            obj.Balance = contaSender.Balance - movimento.Amount;
            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "PUT";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(obj);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    
                    return Ok("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {
                

                return Ok("Updatde do balanço do user" + ex.Message);
            }


            //update no balanço da conta do reciever

            link = String.Format("https://localhost:44360/bank_1/conta/" + contaReciever.Id);

            ContaBankSide obj2 = new ContaBankSide();
            obj2.Id = contaReciever.Id+"";
            obj2.Balance = contaReciever.Balance + movimento.Amount;
            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "PUT";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(obj2);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    return Ok("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return Ok("Updatde do balanço do user" + ex.Message);
            }


            //Movimentos

            //se os bancos forem diferentes enviar duas guias se não enviar apenas umas

            //emitir movimentos sender


            MovimentoBancario registoNoBanco = new MovimentoBancario();
            registoNoBanco.Date = DateTime.Now + "";
            registoNoBanco.IdReceiver = movimento.IdReceiver;
            registoNoBanco.BankRefReceiver = movimento.BankRefReceiver;
            registoNoBanco.IdSender = movimento.IdSender;
            registoNoBanco.BankRefSender = movimento.BankRefSender;
            registoNoBanco.Type = TypeOfMoviment.Debito;
            registoNoBanco.Amount = movimento.Amount;

            link = String.Format("https://localhost:44360/" + readerBankReceiver + "/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(registoNoBanco);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    
                    
                    return Ok("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {
                

                return Ok("Erro ao emitir nota - emitir movimento Api side "+ex.Message+"\n"+ex.StackTrace);
            }

            //emitir movimentos reciever

            link = String.Format("https://localhost:44360/" + readerBankReceiver + "/movimentos");
            registoNoBanco.Type = TypeOfMoviment.Credito;

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(registoNoBanco);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {


                    return Ok("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {


                return Ok("Erro ao emitir nota - emitir movimento Api side " + ex.Message + "\n" + ex.StackTrace);
            }


            return Ok("Transferencia realizada com sucesso");
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}