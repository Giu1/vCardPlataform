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
using System.Windows.Forms;
using vCardPlatform.Models;
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class Pagamento : Form
    {
        private User AuthUser;
        public Pagamento(User conta)
        {
            InitializeComponent();
            this.AuthUser = conta;
        }

        private void Pagamento_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = String.Format("http://localhost:50766/api/movimentos/pagamento");

            vCardPlatform.Models.MovimentoBancario movimentoBancario = new vCardPlatform.Models.MovimentoBancario();

            movimentoBancario.IdSender = AuthUser.Id + "";
            movimentoBancario.IdReceiver = textBox1.Text;
            movimentoBancario.BankRefReceiver = textBox2.Text;
            try
            {
                movimentoBancario.Amount = float.Parse(textBox2.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Erro - Por favor inserir um numero");
            }
            
            

            PasswordForm frm = new PasswordForm(AuthUser);
            if (frm.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                
                string result = JsonConvert.SerializeObject(movimentoBancario);
                HttpWebResponse response = null;
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
                    this.DialogResult = DialogResult.OK;

                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseText = reader.ReadToEnd();
                        MessageBox.Show(responseText.Trim(charsToTrim));
                    }

                    return;
                }


            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Erro: " + ex.Message + "\n" + ex.InnerException);

                return;
            }
        }
    }
}
