using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatform.Models;
using Conta = vCardPlatformAPI.Models.Conta;
using ContaBankSide = vCardPlatform.Models.Conta;

namespace ClientApp
{
    public partial class Depositar : Form
    {
        private Conta authUser;
        private ContaBankSide contaBank;
        public Depositar(Conta conta)
        {
            InitializeComponent();
            this.authUser = conta;

            string link = String.Format("https://localhost:44360/bank_1/conta/" + conta.BankId);

            ContaBankSide obj = null;

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

                obj = (ContaBankSide)serializer.Deserialize(strResul, typeof(ContaBankSide));
                this.contaBank = obj;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            label2.Text = label2.Text + "  "+obj.Balance + " €";
        }

        private void Depositar_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //emitir debito


            string link = String.Format("https://localhost:44360/bank_1/movimentos");

            MovimentoBancario registoNoBanco = new MovimentoBancario();
            try
            {
                registoNoBanco.Amount = float.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Inserir um numero");
                return;
            }
            
            registoNoBanco.Date = DateTime.Now+ "";
            registoNoBanco.IdReceiver = "teste";
            registoNoBanco.BankRefReceiver = "teste";
            registoNoBanco.IdSender = authUser.BankId;
            registoNoBanco.BankRefSender = authUser.BankRef;
            
            
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
                    MessageBox.Show("Erro ao emitir nota");
                    this.DialogResult = DialogResult.None;
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota");

                return;
            }
            

            //update no balanço da conta do user

             link = String.Format("http://localhost:50766/api/conta/" + authUser.Id);

            Conta obj = new Conta();
            obj.Id = authUser.Id;
            obj.Balance = authUser.Balance + registoNoBanco.Amount;
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

            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return;
            }



            //update no balanço da conta do bank do user


            link = String.Format("https://localhost:44360/bank_1/conta/" + contaBank.Id);

            ContaBankSide obj2 = new ContaBankSide();
            obj2.Id = contaBank.Id;
            obj2.Balance = contaBank.Balance - registoNoBanco.Amount;

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

               
            }
            catch (Exception)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return;
            }

            MessageBox.Show("Dinheiro depositado com sucesso!");

            this.DialogResult = DialogResult.OK;
            return;
        }
    }
}
