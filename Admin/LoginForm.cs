using AdminConsole;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace Admin
{
    public partial class LoginForm : Form
    {
        public String email = "";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ToDO Get API Check Credentials
            this.email = textBox1.Text;
            // Query to Main API for Credentials
            // To Do


            //verificar se existe

            string link = String.Format("http://localhost:50766/api/admin/");

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                request.Headers.Add("Custom",email);
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizador não encontrado Local ."+ ex.Message + ex.StackTrace);

                return;
            }


            //password



            //String password = Hash_SHA256(this.textBox2.Text);
            AdminAccount obj = null;

            try
            {
                WebRequest requestPassword = WebRequest.Create(link);
                requestPassword.Method = "GET";
                HttpWebResponse responsePassword = null;
                requestPassword.Headers.Add("Custom", email);
                responsePassword = (HttpWebResponse)requestPassword.GetResponse();

                String strResul = null;
                using (Stream stream = responsePassword.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(stream);
                    strResul = reader.ReadToEnd();
                    reader.Close();
                }

                var serializer = new JavaScriptSerializer();

                obj = (AdminAccount)serializer.Deserialize(strResul, typeof(AdminAccount));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string password = this.textBox2.Text;//Hash_SHA256(this.textBox2.Text);
            string pobj = obj.Password.Trim();
            if (string.Compare(pobj,password)!=0)
            {
                MessageBox.Show("Password incorreta");
                return;

            }
            Global.CurrentUser = obj;
            Form consola = new Admin_Console();
            consola.Show();
            Form loginhere = this;
            loginhere.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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