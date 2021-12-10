using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace Admin
{
    public partial class ChangeXForm : Form
    {
        String change = "";
        AdminAccount User = Global.CurrentUser;
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

                    label5.Text = "Current : "+User.Nome; 
                    label6.Text = User.Nome;  

                    Xlabel.Text = "Name";
                    InsertLabel.Text = "Insert New Name Here";
                    break;
                case "2":       // Change Email

                    label5.Text = "Current : "+User.Email;
                    label6.Text = User.Nome;

                    Xlabel.Text = "Email";
                    InsertLabel.Text = "Insert New Email Here";
                    break;
                case "3":       // Change Password
                    label5.Text = "Current : "+User.Password;
                    label6.Text = User.Nome;

                    Xlabel.Visible = false;
                    labelx2.Visible = true;
                    labelx2.Text = "Password";
                    InsertLabel.Text = "Insert New Password Here";
                    break;
                case "4":       // Change Account
                    label5.Visible = false;
                    label6.Text = User.Nome;

                    Xlabel.Visible = false;
                    labelx2.Visible = true;
                    labelx2.Text = "Account";
                    // Get All Accounts or By ID
                    InsertLabel.Text = "Select Account to Change";
                    break;
                case "5":       // Delete Account
                    label5.Visible = false;
                    label6.Text = User.Nome;


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
                    string status = "";
                    if(User.Enabled == 1)
                    {
                        status = "Enabled";
                    }
                    else if(User.Enabled == 0) { status = "Disabled"; }  
                    
                    label5.Text = "Current :" +status;
                    label6.Text = "Current Account : " + User.Nome;


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

        private void button1_Click(object sender, EventArgs e)
        {
            switch (change)
            {
                case "1":       // Change Name

                    break;
                case "2":       // Change Email

                    break;
                case "3":       // Change Password
                    
                    break;
                case "4":       // Change Account
                    
                    break;
                case "5":       // Delete Account
                   
                    break;
                case "6":       // Enable Disable Account
                    UpdateStatus();
                    User = Global.CurrentUser;
                    
                    break;

            }
        }

        private void UpdateStatus()
        {
            Stream dataStream = null;
            string link = String.Format("http://localhost:50766/api/admin/"+User.Id);
            AdminAccount cur = new AdminAccount();

            try
            {
                cur.Id = User.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            


            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "PUT";
                HttpWebResponse response = null;

                string result = JsonConvert.SerializeObject(cur);

                byte[] data = Encoding.ASCII.GetBytes(result);
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, result.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        MessageBox.Show(reader.ReadToEnd());
                    }
                    this.DialogResult = DialogResult.OK;
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao modificar estado de conta" + ex.Message + "\n" + ex.StackTrace);

                return;
            }




        }
        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
