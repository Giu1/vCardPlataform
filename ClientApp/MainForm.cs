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
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class MainForm : Form
    {

        private static User AuthUser { get; set; }
        public MainForm(User user)
        {
            InitializeComponent();
            AuthUser = user;
            LoadElements();
        }

        private void LoadElements()
        {
            this.label1.Text = AuthUser.AccountOwner;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.label4.Text = AuthUser.Balance + " €";
            if (AuthUser.Photo != null)
            {
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(AuthUser.Photo)));

            }
            else
            {
                this.pictureBox1.Image = (Image)Properties.Resources.defautImage;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            ProfileForm f2 = new ProfileForm(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void ReloadUser()
        {
            string link = String.Format("http://localhost:50766/api/conta/" + AuthUser.Id);
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

            AuthUser = (User)serializer.Deserialize(strResul, typeof(User));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pagamento f2 = new Pagamento(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Depositar f2 = new Depositar(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Levantar f2 = new Levantar(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Comprar f1 = new Comprar(AuthUser);
        }
    }
}
