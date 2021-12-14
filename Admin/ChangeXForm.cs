using AdminConsole;
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
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace Admin
{
    public partial class ChangeXForm : Form
    {
        String change = "";
        AdminAccount User = Global.CurrentUser;
        string[] Accounts = { };
        public ChangeXForm(String x)
        {
            InitializeComponent();
            change = x;
            comboBox1.Visible = false;
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
                    label5.Visible = true;
                    textBox1.Visible = false;
                    label5.Text = User.Nome;
                    label6.Visible = false;
                    Xlabel.Visible = false;
                    labelx2.Visible = true;
                    labelx2.Text = "Account";
                    // Get All Accounts or By ID
                    InsertLabel.Text = "Select Account to Change";

                    GetAllAccounts();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Accounts;


                    break;
                case "5":       // Delete Account
                    label5.Visible = false;
                    button1.Text = "Delete";
                    textBox1.Visible = false;
                    label6.Text = User.Nome;
                    labelx2.Text = "Account";
                    labelx2.Visible = true;
                    Xlabel.Visible = false;
                    x2Label.Text = "Deletion Menu";
                    x2Label.Visible = true;
                    // Get All Accounts or By ID
                    InsertLabel.Text = "Are you sure you want to delete\n this account? After this \naction is completed the data \nis lost for good!!!!";
                    textBox1.Visible = false;
                    
                    GetAllAccounts();
                    comboBox1.Visible = true;
                    comboBox1.DataSource = Accounts;

                    break;
                case "6":       // Enable Disable Account
                    string status = "";
                    if(User.Enabled == 1)
                    {
                        status = "Enabled";
                    }
                    else if(User.Enabled == 0) { status = "Disabled"; }  
                    
                    label5.Text ="Admin :"+ User.Nome;
                    label6.Visible = false;


                    // Get API For Status of Account
                    Xlabel.Text = "Account";
                    InsertLabel.Text = " This Account is "+status;
                    textBox1.Visible = false;
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
            Form console = new Admin_Console();
            console.Show();
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
            string pass = textBox2.Text;
            switch (change)
            {
                case "1":       // Change Name
                    UpdateNome(pass);
                    break;
                case "2":       // Change Email
                    UpdateEmail(pass);
                    break;
                case "3":       // Change Password
                    UpdatePassword(pass);
                    break;
                case "4":       // Change Account                
                    ChangeAccount(pass);
                    break;
                case "5":       // Delete Account
                    DeleteAccount(pass);
                    comboBox1.DataSource = Accounts;
                    break;
                case "6":       // Enable Disable Account
                    UpdateStatus(pass);
                    User = Global.CurrentUser;
                    break;

            }
        }

        private void UpdateStatus(String pass)
        {
            
            string link = String.Format("http://localhost:50766/api/admin/status/"+User.Id);
            AdminAccount cur = User;
            if (cur.Enabled == 1) { cur.Enabled = 0; } else { cur.Enabled = 1; }

            if (cur.Password == pass)
            {
                try
                {
                    WebRequest request = WebRequest.Create(link);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
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
                        this.DialogResult = DialogResult.OK;
                        User = cur;
                        MessageBox.Show("Alterações realizadas com sucesso");
                        if (cur.Enabled == 1) { InsertLabel.Text = " This Account is Enabled"; }
                        else
                        {
                            InsertLabel.Text = " This Account is Disabled";
                        }
                        return;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao modificar estado de conta" + ex.Message + "\n" + ex.StackTrace);
                    if (cur.Enabled == 1) { cur.Enabled = 0; } else { cur.Enabled = 1; }
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }
        }

        private void UpdateNome(String pass)
        {

            string link = String.Format("http://localhost:50766/api/admin/name/" + User.Id);
            AdminAccount cur = User;

           

            if (cur.Password == pass)
            {
                try
                {
                    cur.Nome = textBox1.Text;

                    WebRequest request = WebRequest.Create(link);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
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
                        this.DialogResult = DialogResult.OK;
                        User = cur;
                        MessageBox.Show("Alterações realizadas com sucesso");
                        label5.Text = User.Nome;
                        label6.Text = User.Nome;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao modificar estado de conta" + ex.Message + "\n" + ex.StackTrace);
                    if (cur.Enabled == 1) { cur.Enabled = 0; } else { cur.Enabled = 1; }
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }
        }
        
        private void UpdateEmail(String pass)
        {

            string link = String.Format("http://localhost:50766/api/admin/email/" + User.Id);
            AdminAccount cur = User;



            if (cur.Password == pass)
            {
                try
                {
                    cur.Email = textBox1.Text;

                    WebRequest request = WebRequest.Create(link);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
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
                        this.DialogResult = DialogResult.OK;
                        User = cur;
                        MessageBox.Show("Alterações realizadas com sucesso");
                        label5.Text = User.Nome;
                        label6.Text = User.Email;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao modificar estado de conta" + ex.Message + "\n" + ex.StackTrace);
                    if (cur.Enabled == 1) { cur.Enabled = 0; } else { cur.Enabled = 1; }
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }
        }
       
        private void UpdatePassword(String pass)
        {

            string link = String.Format("http://localhost:50766/api/admin/password/" + User.Id);
            AdminAccount cur = User;



            if (cur.Password == pass)
            {
                try
                {
                    cur.Password = textBox1.Text;

                    WebRequest request = WebRequest.Create(link);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
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
                        this.DialogResult = DialogResult.OK;
                        User = cur;
                        MessageBox.Show("Alterações realizadas com sucesso");
                        label5.Text = User.Nome;
                        label6.Text = User.Password;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao modificar estado de conta" + ex.Message + "\n" + ex.StackTrace);
                    if (cur.Enabled == 1) { cur.Enabled = 0; } else { cur.Enabled = 1; }
                    return;
                }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }
        }

        private void GetAllAccounts()
        {
            string link = String.Format("http://localhost:50766/api/admin/admins/");
            AdminAccount cur = null;
          
                try
                {
                    WebRequest request = WebRequest.Create(link);
                    request.Method = "GET";
                    HttpWebResponse response = null;
                    response = (HttpWebResponse)request.GetResponse();


                    String strResul = null;
                using (Stream stream = response.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(stream);
                    strResul = reader.ReadToEnd();
                    reader.Close();
                }

                var serializer = new JavaScriptSerializer();

                Accounts = (String[])serializer.Deserialize(strResul, typeof(String[]));

            }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao buscar as contas" + ex.Message + "\n" + ex.StackTrace);
                   
                    return;
                }
            }

        private void ChangeAccount(string pass)
        {
            int selectedItem = comboBox1.SelectedIndex+1;

            string link = String.Format("http://localhost:50766/api/admin/"+selectedItem);
            
            if (User.Password == pass)
            {
                try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                HttpWebResponse response = null;
                response = (HttpWebResponse)request.GetResponse();


                String strResul = null;
                using (Stream stream = response.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(stream);
                    strResul = reader.ReadToEnd();
                    reader.Close();
                }

                var serializer = new JavaScriptSerializer();

                AdminAccount acc = (AdminAccount)serializer.Deserialize(strResul, typeof(AdminAccount));
                User = acc;
                Global.CurrentUser = acc;
                MessageBox.Show("Sucesso Conta Mudada");
                label5.Text = User.Nome;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro Ao Mudar de Conta" + ex.Message + "\n" + ex.StackTrace);

                return;
            }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }

        }

        private void DeleteAccount(string pass)
        {
            int selectedItem = comboBox1.SelectedIndex + 1;

            string link = String.Format("http://localhost:50766/api/admin/delete/"+selectedItem);

            if (User.Password == pass)
            {
                if (User.Id == selectedItem.ToString())
                {
                    MessageBox.Show("Erro : Nao Permitido Eliminar Conta Logada");

                    return;
                }
                else { 
                try
                {
                        WebRequest request = WebRequest.Create(link);
                        request.Method = "DElETE";
                        HttpWebResponse response = null;

                        response = (HttpWebResponse)request.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            MessageBox.Show("Sucesso Conta Eliminada");
                            GetAllAccounts(); 

                        }
                        

                    }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro Ao Mudar de Conta" + ex.Message + "\n" + ex.StackTrace);

                    return;
                }
            }
            }
            else
            {
                MessageBox.Show("Erro : Password Errada");
                return;
            }


        }
    }

}

