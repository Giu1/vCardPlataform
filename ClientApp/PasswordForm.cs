using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class PasswordForm : Form
    {
        private User user;
        public PasswordForm(User AuthUser)
        {
            InitializeComponent();
            this.user = AuthUser;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string password = Hash_SHA256(textBox1.Text);
            if (Hash_SHA256(textBox1.Text).CompareTo(user.Password) == 0)
            {
                // The password is ok.
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // The password is invalid.
                textBox1.Clear();
                MessageBox.Show("Password incorreta");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
