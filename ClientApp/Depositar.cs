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
using User = vCardPlatformAPI.Models.User;
using ContaBankSide = vCardPlatform.Models.Conta;

namespace ClientApp
{
    public partial class Depositar : Form
    {
        private User authUser;
        private ContaBankSide contaBank;
        public Depositar(User conta)
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
            //emitir movimento bank side

            string link = String.Format("http://localhost:50766/api/movimentos/depositar");

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

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        MessageBox.Show(reader.ReadToEnd());
                    }
                    this.DialogResult = DialogResult.OK;
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace);

                return;
            }


        }
    }
    
}
