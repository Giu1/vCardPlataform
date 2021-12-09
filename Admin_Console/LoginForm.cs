using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin_Console
{
    public partial class LoginForm : Form
    {
        String email = "";
        String password = "";
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            email = label1.Text;
            password = label2.Text;
            var f = new WindowsApplication1 .Form1();
f.Show();
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.SolutionFolder(), @"Admin_Console\bin\debug\AdminConsole.exe"));

        }
    }
}
