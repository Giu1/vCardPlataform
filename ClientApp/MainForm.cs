using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vCardPlatform.Models;

namespace ClientApp
{
    public partial class MainForm : Form
    {

        private static User AuthUser { get; set; }
        public MainForm(User user)
        {
            InitializeComponent();
            AuthUser = user;
            this.label1.Text = AuthUser.AccountOwner;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (user.Photo != null)
            {
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(user.Photo)));
            }
            else
            {
                //this.pictureBox1.Image = (Image)Properties.Resources.head_the_dummy_avatar_man_tie_72756;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            ProfileForm f2 = new ProfileForm(AuthUser);

            f2.Show();



        }


    }
}
