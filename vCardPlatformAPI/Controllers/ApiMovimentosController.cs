﻿using Newtonsoft.Json;
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
using vCardPlatformAPI.Models;

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

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetMovimento(string id)
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
                    pedidoAInserir.IdReceiver = (string)reader["Id_Receiver"];
                    pedidoAInserir.Amount = (float)reader["Amount"];
                    pedidoAInserir.Date = (string)reader["Date"];

                    if (reader["Description"].Equals(System.DBNull.Value))
                    {
                        pedidoAInserir.Description = "";
                    }
                    else
                    {
                        pedidoAInserir.Description = (string)reader["Description"];
                    }

                    pedidoAInserir.Type = (TypeOfMoviment)Enum.Parse(typeof(TypeOfMoviment), (string)reader["Type"]);

                }

                reader.Close();
                connection.Close();
                if (pedidoAInserir != null)
                {
                    return Ok(pedidoAInserir);
                }
                else
                {
                    return BadRequest("Elemento não encontrado");
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

        //get all movimentos by user id
        [Route("user/{id}")]
        [HttpGet]
        public IHttpActionResult GetMovimentoByUserId(string id)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Movimentos WHERE Id_Receiver=@idReceiver OR Id_Sender=@idSender";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idReceiver", id);
                command.Parameters.AddWithValue("@idSender", id);
                SqlDataReader reader = command.ExecuteReader();

                MovimentoBancario pedidoAInserir = null;
                List<MovimentoBancario> movimentoBancarios = new List<MovimentoBancario>();

                while (reader.Read())
                {
                    pedidoAInserir = new MovimentoBancario();
                    pedidoAInserir.Id = (string)reader["Id"];
                    pedidoAInserir.IdSender = (string)reader["Id_Sender"];
                    pedidoAInserir.IdReceiver = (string)reader["Id_Receiver"];
                    pedidoAInserir.Amount = (float)reader["Amount"];
                    pedidoAInserir.Date = (string)reader["Date"];

                    if (reader["Description"].Equals(System.DBNull.Value))
                    {
                        pedidoAInserir.Description = "";
                    }
                    else
                    {
                        pedidoAInserir.Description = (string)reader["Description"];
                    }

                    pedidoAInserir.Type = (TypeOfMoviment)Enum.Parse(typeof(TypeOfMoviment), (string)reader["Type"]);
                    movimentoBancarios.Add(pedidoAInserir);

                }

                reader.Close();
                connection.Close();
                if (movimentoBancarios.Count !=0)
                {
                    return Ok(movimentoBancarios);
                }
                else
                {
                    return Ok("Erro - Não existem elementos nesta colecao");
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
        public IHttpActionResult RegistarMovimentos([FromBody] Movimento movimento)
        {
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            connection.Open();
            string cmdSQL = null;
            SqlCommand command = new SqlCommand(cmdSQL, connection);


            try
            {

                cmdSQL = "INSERT INTO Movimentos(Id,Id_Sender,Id_Receiver,Amount,Description,Date,Type) VALUES (@z,@a,@c,@e,@f,@g,@h)";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@z", DateTime.Now.Ticks + "");
                command.Parameters.AddWithValue("@a", movimento.IdSender);
                command.Parameters.AddWithValue("@c", movimento.IdReceiver);

                if (movimento.Amount > 0)
                {
                    command.Parameters.AddWithValue("@e", movimento.Amount);
                }
                else
                {
                    return BadRequest("Erro um movimento não tem valores negativos.");
                }
                command.Parameters.Add(new SqlParameter("@f", string.IsNullOrEmpty(movimento.Description) ? (object)DBNull.Value : movimento.Description));
                string aqui = movimento.Type.ToString();
                command.Parameters.AddWithValue("@g", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@h", movimento.Type.ToString());

                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok("Sucesso");
                }

                return Ok("Erro - Id não encontrado");

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

        [Route("comprar")]
        [HttpPost]
        public IHttpActionResult Comprar([FromBody] MovimentoBancario movimentoBancario)
        {
            Movimento movimento = new Movimento();
            movimento.IdSender = movimentoBancario.IdSender;
            movimentoBancario.IdSender = "1111111111111"; //referencia para conta do banco da plataforma
            movimentoBancario.BankRefSender = "44360";

            string link = String.Format("http://localhost:50766/api/movimentos/trans");

            //emite transferencia entre a entidade e a plataforma

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

                string responseText;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                     responseText = reader.ReadToEnd();
                    
                }

                if (responseText != "Suceesso")
                {
                    return Ok(responseText);
                }
            }
            catch (Exception ex)
            {

                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message + "\n" +ex.StackTrace);
                //return Ok("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace);
            }


            //atualiza saldo na api
            SqlConnection connection = null;
            SqlCommand command = null;


            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();


                string cmdSQL = "UPDATE Contas set Balance=Balance - @amount WHERE BankID = @id";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@id", movimento.IdSender);

                command.Parameters.AddWithValue("@amount", movimentoBancario.Amount);


                if (!(command.ExecuteNonQuery() > 0))
                {
                    connection.Close();
                    
                    return Ok("Erro ao atualizar saldo - Não foi encontrada a referencia inserida");
                }

            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                return Ok("Erro ao atualizar saldo - ID Reciever"+ ex.Message + ex.StackTrace);
                //return BadRequest(e.Message + e.StackTrace);
            }

            //regista movimento 

            //emitir movimentos sender

            movimentoBancario.Type = TypeOfMoviment.Debito;

            
            movimento.IdReceiver = movimentoBancario.IdReceiver;
            movimento.IdSender = movimentoBancario.IdSender;
            movimento.Type = movimentoBancario.Type;
            movimento.Id = movimentoBancario.Id;
            movimento.Amount = movimentoBancario.Amount;
            movimento.Description = movimentoBancario.Description;
            movimento.Date = movimentoBancario.Date;

            link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimento);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    return Ok("Erro ao emitir nota de credito");
                    //return BadRequest("Erro ao emitir nota - emitir movimento Api side");
                }

            }
            catch (Exception ex)
            {

                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message + "\n" + ex.StackTrace);
                //return BadRequest("Erro ao emitir nota - emitir movimento Api side " + ex.Message + "\n" + ex.StackTrace);
            }

            return new HttpActionResult(HttpStatusCode.OK, "Depósito realizado com sucesso");
            //return Ok("Depósito realizado com sucesso");
        }

        [Route("pagamento")]
        [HttpPost]
        public IHttpActionResult PagamentoNaApp([FromBody] Movimento movimento)
        {
            //verificar sender
            SqlConnection connection = null;
            User helper = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE PhoneNumber=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.IdSender);
                SqlDataReader reader = command.ExecuteReader();

                User sender = null;

                while (reader.Read())
                {
                    sender = new User
                    {
                        Id = (int)reader["PhoneNumber"],
                        Balance = (float)reader["Balance"],
                        ConfirmationCode = (int)reader["ConfirmationCode"]
                    };
                }
                reader.Close();
                connection.Close();
                helper = sender;

                if (sender == null)
                {
                    return BadRequest("Erro - Id não encontrado");
                }
                if (sender.Balance < movimento.Amount)
                {
                    return BadRequest("Sender nao tem dinheiro suficiente");
                }
               
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return BadRequest(e.Message + e.StackTrace);
            }


            //verificar receiver

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE PhoneNumber=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", movimento.IdReceiver);
                SqlDataReader reader = command.ExecuteReader();

                User receiver = null;

                while (reader.Read())
                {
                    receiver = new User();
                    receiver.Id = (int)reader["PhoneNumber"];
                    receiver.Balance = (float)reader["Balance"];
                    receiver.ConfirmationCode = (int)reader["ConfirmationCode"];
                }
                reader.Close();
                connection.Close();

                if (receiver == null)
                {
                    return Ok("Erro - Id não encontrado");
                }
                if (receiver.Id == helper.Id)
                {
                    return Ok("Erro - Sender e Receiver teem o mesmo id");
                }
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return BadRequest(e.Message + e.StackTrace);
            }

            //update saldo sender

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "UPDATE Contas SET Balance = Balance - @amount WHERE PhoneNumber=@id; ";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@amount", movimento.Amount);
                command.Parameters.AddWithValue("@id", movimento.IdSender);

                if (!(command.ExecuteNonQuery() > 0))
                {
                    connection.Close();
                    return BadRequest("Erro ao atualizar saldo");
                }
                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }

            //update saldo receiver

            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "UPDATE Contas SET balance = balance + @amount WHERE PhoneNumber=@id; ";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@amount", movimento.Amount);
                command.Parameters.AddWithValue("@id", movimento.IdReceiver);
              

                if (!(command.ExecuteNonQuery() > 0))
                {
                    connection.Close();
                    return BadRequest("Erro ao atualizar saldo");
                }
                connection.Close();
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + e.StackTrace);
            }

            //emitir movimento sender
            movimento.Type = TypeOfMoviment.Debito;


            String link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimento);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseText;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        responseText = reader.ReadToEnd();
                    }
                    if (responseText.CompareTo("\"Sucesso\"") !=0)
                    {
                        return Ok(responseText);
                    }
                    
                }

            }
            catch (Exception ex)
            {


                return Ok("Erro ao emitir nota - emitir movimento Api side " + ex.Message + "\n" + ex.StackTrace);
            }
            //emitir movimento receiver
            movimento.Type = TypeOfMoviment.Credito;


            link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimento);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseText;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        responseText = reader.ReadToEnd();
                    }
                    if (responseText.CompareTo("\"Sucesso\"") != 0)
                    {
                        return Ok(responseText);
                    }

                }

            }
            catch (Exception ex)
            {


                return Ok("Erro ao emitir nota - emitir movimento Api side " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok("Sucesso");
        }

        [Route("levantar")]
        [HttpPost]
        public IHttpActionResult Levantar([FromBody] MovimentoBancario movimentoBancario)
        {
            movimentoBancario.IdSender = "1111111111111"; //referencia para conta do banco da plataforma
            movimentoBancario.BankRefSender = "1111";

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


            connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();


                string cmdSQL = "UPDATE Contas set Balance=Balance - @amount WHERE BankID = @id";
                command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@id", movimentoBancario.IdReceiver);

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

            Movimento movimento = new Movimento();
            movimento.IdReceiver = movimentoBancario.IdReceiver;
            movimento.IdSender = movimentoBancario.IdSender;
            movimento.Type = movimentoBancario.Type;
            movimento.Id = movimentoBancario.Id;
            movimento.Amount = movimentoBancario.Amount;
            movimento.Description = movimentoBancario.Description;
            movimento.Date = movimentoBancario.Date;

            link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimento);

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
                
                return Ok("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace+" "+ex.InnerException);
            }
            

            //atualiza saldo na api
            SqlConnection connection = null;
            SqlCommand command = null;
            

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

            Movimento movimento = new Movimento();
            movimento.IdReceiver = movimentoBancario.IdReceiver;
            movimento.IdSender = movimentoBancario.IdSender;
            movimento.Type = movimentoBancario.Type;
            movimento.Id = movimentoBancario.Id;
            movimento.Amount = movimentoBancario.Amount;
            movimento.Description = movimentoBancario.Description;
            movimento.Date = movimentoBancario.Date;


            link = String.Format("http://localhost:50766/api/movimentos");


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(movimento);

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
            

            //TODO check other variables
            //check user_sender
            if (movimento.Amount<0)
            {
                return Ok("Transferencia não pode ser negativa");
            }

           

            //get conta sender
            string linkSave = String.Format("https://localhost:"+movimento.BankRefSender+"/conta/"+movimento.IdSender);
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

                if (contaSender.Balance <0)
                {
                    return Ok( "Sender não tem dinheiro");
                    //return BadRequest();
                }
                if (contaSender.Balance < movimento.Amount)
                {
                    return Ok("Sender não tem dinheiro suficiente");
                    //Content(HttpStatusCode.BadRequest, );
                    //return BadRequest("Sender não tem dinheiro suficiente");
                }
            }
            catch (Exception ex)
            {

                Ok("Erro: " + ex.Message + "\n" + ex.StackTrace);
            }

            //check user_receiver

          

            //get conta receiver

            link = String.Format("https://localhost:44360/" + movimento.BankRefReceiver + "/conta/" + movimento.IdReceiver);
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
                try
                {
                    contaReciever = (ContaBankSide)serializer.Deserialize(strResul, typeof(ContaBankSide));
                }
                catch (Exception)
                {

                    return Ok(strResul);
                }
                    
                
               
                
            }
            catch (Exception ex)
            {


                return Ok("Erro: " + ex.Message + "\n" + ex.StackTrace);
            }




            //alterar saldos

            //update no balanço da conta do sender

            link = String.Format("https://localhost:" + movimento.BankRefSender + "/conta/" + contaSender.Id);

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


                Ok("Erro: " + ex.Message + "\n" + ex.StackTrace);
            }


            //update no balanço da conta do reciever

            link = String.Format("https://localhost:" + movimento.BankRefSender + "/conta/" + contaReciever.Id);

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


            //emitir movimentos sender


            MovimentoBancario registoNoBanco = new MovimentoBancario();
            registoNoBanco.Date = DateTime.Now + "";
            registoNoBanco.IdReceiver = movimento.IdReceiver;
            registoNoBanco.BankRefReceiver = movimento.BankRefReceiver;
            registoNoBanco.IdSender = movimento.IdSender;
            registoNoBanco.BankRefSender = movimento.BankRefSender;
            registoNoBanco.Type = TypeOfMoviment.Debito;
            registoNoBanco.Amount = movimento.Amount;

            link = String.Format("https://localhost:" + movimento.BankRefSender + "/movimentos");


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

            link = String.Format("https://localhost:" + movimento.BankRefReceiver + "/movimentos");
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


            return Ok("Sucesso");
        }

        
        
    }
}