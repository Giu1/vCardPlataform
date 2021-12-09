using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class ProfileForm : Form
    {
        private static User AuthUser { get; set; }
        public ProfileForm(User user)
        {
            InitializeComponent();
            AuthUser = user;
            ReloadElements();

        }

        public void ReloadElements()
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (AuthUser.Photo != null)
            {
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(AuthUser.Photo)));
            }
            else
            {
                this.pictureBox1.Image = (Image)Properties.Resources.defautImage;
            }

            this.label1.Text = AuthUser.Email;
            this.label3.Text = AuthUser.AccountOwner;
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = AuthUser;

            if (this.textBox2.Text.Length > 0 && this.textBox2.Text.Length < 50)
            {
                user.AccountOwner = textBox2.Text;
            }

            //TODO - check email format
            if (this.textBox7.Text.Length > 0)
            {
                user.Email = textBox7.Text;
            }

            bool checkPassword = false;
            if (textBox3.Text.Length > 0)
            {


                if (textBox3.Text.CompareTo(textBox4.Text) == 0)
                {
                    user.Password = Hash_SHA256(textBox3.Text);
                    checkPassword = true;

                }
                else
                {
                    MessageBox.Show("Passwords não são iguais");
                    return;
                }
            }

            if (textBox5.Text.Length > 0)
            {

                if (textBox5.Text.Length != 4)
                {
                    MessageBox.Show("ConfirmationCode apenas pode ter 4 digitos");
                    return;
                }

                try
                {
                    int confirmationCode = Int16.Parse(textBox5.Text);
                    user.ConfirmationCode = confirmationCode;
                    checkPassword = true;


                }
                catch (FormatException)
                {

                    MessageBox.Show("Inserir apenas numeros.");
                    return;
                }

            }

            if (checkPassword == true)
            {
                PasswordForm frm = new PasswordForm(AuthUser);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    // The user canceled.
                    this.Close();
                }
                
            }

            string link = String.Format("http://localhost:50766/api/conta/" + user.Id);

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "PUT";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(user);

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
                    AuthUser = user;
                    MessageBox.Show("Alterações realizadas com sucesso");
                    return;

                }

            }
            catch (Exception)
            {
                MessageBox.Show("Utilizador não encontrado.");

                return;
            }


        }



        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.png)|*.png|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            byte[] image = File.ReadAllBytes(filePath);


            User user = new User();
            //user.Photo = System.Text.Encoding.UTF8.GetString(image); 
            user.Id = AuthUser.Id;

            string link = String.Format("http://localhost:50766/api/conta/" + AuthUser.Id);

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "PUT";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                user.Photo = Convert.ToBase64String(image);
                string result = JsonConvert.SerializeObject(user);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //refresh elements //TODO
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    MemoryStream mem = new MemoryStream(image);
                    pictureBox1.Image = Image.FromStream(mem);
                    AuthUser.Photo = user.Photo;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizador não encontrado." + ex.Message);

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
