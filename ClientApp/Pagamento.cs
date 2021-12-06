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
        private Conta conta;
        public Pagamento(Conta conta)
        {
            InitializeComponent();
            this.conta = conta;
        }

        private void Pagamento_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = String.Format("http://localhost:50766/api/movimento/pagamento");

            MovimentoBancario movimentoBancario = new MovimentoBancario();

            movimentoBancario.IdSender = conta.Id+"";
            movimentoBancario.IdReceiver = textBox1.Text;
            movimentoBancario.Amount = float.Parse(textBox2.Text);

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

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Alterações realizadas com sucesso");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return;
            }
        }
    }
}
