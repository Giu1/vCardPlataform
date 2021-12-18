using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatform.Models;
using vCardPlatformAPI.Models;
using ContaBankSide = vCardPlatform.Models.Conta;

namespace ClientApp
{
    public partial class Levantar : Form
    {

        private User authUser;
        private ContaBankSide contaBank;
        public Levantar(User conta)
        {
            InitializeComponent();
            this.authUser = conta;

            ContaBankSide obj = null;
            Bank obj2 = null;

            //get bank Sender
            string readerBank = null;

            string linkSave = String.Format("http://localhost:50766/api/bank/" + conta.BankRef);
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

                obj2 = (Bank)serializer.Deserialize(strResul, typeof(Bank));


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            linkSave = String.Format("https://localhost:" + conta.BankRef + "/" + obj2.Name + "/conta/" + conta.BankId);
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

                obj = (ContaBankSide)serializer.Deserialize(strResul, typeof(ContaBankSide));
                this.contaBank = obj;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            label2.Text = label2.Text + "  " + obj.Balance + " €";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //emitir pagamento incluindo bank side

            string link = String.Format("http://localhost:50766/api/movimentos/levantar");

            MovimentoBancario registoNoBanco = new MovimentoBancario();

            try
            {
                registoNoBanco.Amount = float.Parse(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            registoNoBanco.IdReceiver = authUser.BankId;
            registoNoBanco.BankRefReceiver = authUser.BankRef;


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


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    char[] charsToTrim = { '"', '\'' };


                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseText = reader.ReadToEnd();
                        if (responseText != "\"Sucesso\"")
                        {
                            MessageBox.Show(responseText);
                        }
                        else
                        {
                            MessageBox.Show(responseText);
                            this.DialogResult = DialogResult.OK;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace);

                return;
            }


        }

        private void Levantar_Load(object sender, EventArgs e)
        {

        }
    }
}
