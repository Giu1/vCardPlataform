using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admin
{
    public partial class ChangeXForm : Form
    {
        String change = "";
        public ChangeXForm(String x)
        {
            InitializeComponent();
            change = x;
        }

        private void ChangeXForm_Load(object sender, EventArgs e)
        {
            switch (change)
            {
                case "1":       // Change Name

                    label5.Text = ""; // current X TO ADD ALL
                    label6.Text = "Current Account : ";  // current Account TO DO ALL

                    Xlabel.Text = "Name";
                    InsertLabel.Text = "Insert New Name Here";
                    break;
                case "2":       // Change Email

                    Xlabel.Text = "Email";
                    InsertLabel.Text = "Insert New Email Here";
                    break;
                case "3":       // Change Password
                    Xlabel.Visible = false;
                    labelx2.Visible = true;
                    labelx2.Text = "Password";
                    InsertLabel.Text = "Insert New Password Here";
                    break;
                case "4":       // Change Account
                    Xlabel.Visible = false;
                    labelx2.Visible = true;
                    labelx2.Text = "Account";
                    // Get All Accounts or By ID
                    InsertLabel.Text = "Select Account to Change";
                    break;
                case "5":       // Delete Account

                    labelx2.Text = "Account";
                    labelx2.Visible = true;
                    Xlabel.Visible = false;
                    x2Label.Text = "Deletion Menu";
                    x2Label.Visible = true;
                    // Get All Accounts or By ID
                    InsertLabel.Text = "Are you sure you want to delete\n this account? After this \naction is completed the data \nis lost for good!!!!";
                    textBox1.Visible = false;
                    break;
                case "6":       // Enable Disable Account

                    // Get API For Status of Account
                    Xlabel.Text = "Account";
                    InsertLabel.Text = " This Account is X/X ( Enabled/Disabled ) ";
                    textBox1.Visible = false;
                    label6.Visible = false;
                    break;

            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        // Close Form and Cancel Change
        private void button2_Click(object sender, EventArgs e)
        {
            Form change = this;
            change.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
