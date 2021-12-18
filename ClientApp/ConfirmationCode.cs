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
    public partial class ConfirmationCode : Form
    {
        private User user;
        public ConfirmationCode(User AuthUser)
        {
            InitializeComponent();
            this.user = AuthUser;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //emitir pagamento incluindo bank side
            int helper;
            try
            {
                helper = int.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Inserir um numero com 4 digitos");
                return;
                
            }

            string link = String.Format("http://localhost:50766/api/movimentos/confirmation/"+user.Id+"/"+helper);


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
               
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
                MessageBox.Show("Erro" + ex.Message + "\n" + ex.StackTrace);

                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void ConfirmationCode_Load(object sender, EventArgs e)
        {

        }
    }
}
