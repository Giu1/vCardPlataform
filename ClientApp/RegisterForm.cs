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
            Conta obj = null;
            String phoneNumber = this.textBoxPhoneNumber.Text;
            String accountowner = textBoxAccountOwner.Text;
            String email = this.textBoxEmail.Text;
            String confirmationcode = textBoxConfirmationCode.Text;
            String password = Hash_SHA256(this.textBoxPassword.Text);

            if (!Int32.TryParse(phoneNumber, out int id) || phoneNumber.Length != 9)
            {
                MessageBox.Show("Por favor inserir 9 numeros.\n (Não inserir codigo de país.)");
                return;
            }
            if (!Int32.TryParse(confirmationcode, out int code) || phoneNumber.Length != 9)
            {
                MessageBox.Show("Por favor inserir 9 numeros.\n (Não inserir codigo de país.)");
                return;
            }

            if (accountowner.Length == 0)
            {
                MessageBox.Show("Por favor inserir um nome.\n");
                return;
            }

            if (email.Length == 0)
            {
                MessageBox.Show("Por favor inserir um email.\n");
                return;
            }

            if (confirmationcode.Length != 4)
            {
                MessageBox.Show("Por favor inserir 4 numeros para o codigo.\n");
                return;
            }

            if (password.Length >= 3)
            {
                MessageBox.Show("Por favor inserir pelo menos 3 caracters para a password.\n");
                return;
            }

            obj.Id = id;
            obj.AccountOwner = accountowner;
            obj.Email = email;
            obj.ConfirmationCode = code;
            obj.Password = password;

            string link = String.Format("http://localhost:50766/api/conta/" + phoneNumber);

            //T

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();

                MessageBox.Show("Utilizador Já Existe.");

                return;
            }
            catch (Exception){}

            link = String.Format("http://localhost:50766/api/conta/");

            try
            {
                WebRequest requestCreat = WebRequest.Create(link);
                requestCreat.Method = "POST";
                HttpWebResponse response = null;

                response = (HttpWebResponse)requestCreat.GetResponse();

                
            }
            catch (Exception)
            {
                MessageBox.Show("Falha ao criar conta");
                return;
            }

        }

        public static string Hash_SHA256(string input)
        {
            using (SHA256Managed sha1 = new SHA256Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
