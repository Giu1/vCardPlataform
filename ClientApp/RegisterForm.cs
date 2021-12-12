using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using vCardPlatformAPI.Models;
using Newtonsoft.Json;

namespace ClientApp
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            User obj = new User
            {
                Id = int.Parse(textBoxPhoneNumber.Text),
                AccountOwner = textBoxAccountOwner.Text,
                Email = this.textBoxEmail.Text,
                ConfirmationCode = int.Parse(textBoxConfirmationCode.Text),
                Password = LoginForm.Hash_SHA256(this.textBoxPassword.Text)
            };

            if (obj.Id <= 900000000 || obj.Id >= 999999999)
            {
                MessageBox.Show("Por favor inserir numeros telemovel Português.\n");
                return;
            }

            if (obj.ConfirmationCode <= 0000 || obj.ConfirmationCode >=9999)
            {
                MessageBox.Show("Por favor inserir 4 numeros para o codigo.\n");
                return;
            }

            if (obj.AccountOwner.Length == 0)
            {
                MessageBox.Show("Por favor inserir um nome.\n");
                return;
            }

            if (obj.Email.Length == 0)
            {
                MessageBox.Show("Por favor inserir um email.\n");
                return;
            }

            if (obj.Password.Length <= 3)
            {
                MessageBox.Show("Por favor inserir pelo menos 3 caracters para a password.\n");
                return;
            }

            string link = String.Format("http://localhost:50766/api/conta/" + obj.Id);

            //T
            /*
            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();

                MessageBox.Show("Utilizador Já Existe.");

                return;
            }
            catch (Exception) { }
            */

            link = String.Format("http://localhost:50766");
            var contaInJson = JsonConvert.SerializeObject(obj);

            try
            {
                var client = new RestSharp.RestClient(link);
                var request = new RestSharp.RestRequest("api/conta/", RestSharp.Method.POST);
                request.AddJsonBody(contaInJson);

                RestSharp.IRestResponse response = client.Execute(request);
                MessageBox.Show(response.StatusCode.ToString());

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    LoginForm LogForm = new LoginForm();
                    this.Hide();

                    LogForm.ShowDialog();

                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falha ao criar conta");
                return;
            }
        }
    }
}
