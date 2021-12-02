using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatform.Models;

namespace ClientApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String phoneNumber;

            phoneNumber = this.textBox1.Text;

            if (phoneNumber.Length != 9)
            {
                MessageBox.Show("Por favor inserir 9 numeros.\n (Não inserir codigo de país.)");
                return;
            }

            //verificar se existe

            string link = String.Format("http://localhost:50766/api/conta/" + phoneNumber);

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return;
            }


            //password



            //String password = Hash_SHA256(this.textBox2.Text);
            User obj = null;

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

                obj = (User)serializer.Deserialize(strResul, typeof(User));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            String password = Hash_SHA256(this.textBox2.Text);
            if (obj.Password.CompareTo(password) != 0)
            {
                MessageBox.Show("Password incorreta");
                return;

            }

            MainForm myForm = new MainForm(obj);
            this.Hide();

            myForm.ShowDialog();

            this.Close();

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

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
