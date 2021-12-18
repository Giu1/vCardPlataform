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
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class Comprar : Form
    {
        private User authUser;
        public Comprar(User authUser)
        {
            InitializeComponent();
            this.authUser = authUser;
        }

        private void Comprar_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = String.Format("http://localhost:50766/api/movimentos/comprar");

            vCardPlatform.Models.MovimentoBancario movimentoBancario = new vCardPlatform.Models.MovimentoBancario();

            movimentoBancario.IdSender = authUser.Id + "";
            movimentoBancario.IdReceiver = textBox1.Text;
            
            movimentoBancario.BankRefReceiver = textBox3.Text;
            try
            {
                movimentoBancario.Amount = float.Parse(textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro - Inserir um numero valido");
                
                return;
            }

            ConfirmationCode frm = new ConfirmationCode(authUser);
            if (frm.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }


            HttpWebResponse response = null;
            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";

                string result = JsonConvert.SerializeObject(movimentoBancario);
                
                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    char[] charsToTrim = {'"', '\'' };
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
