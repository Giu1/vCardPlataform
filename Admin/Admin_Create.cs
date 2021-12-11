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
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using vCardPlatformAPI.Models;

namespace Admin
{
    public partial class Admin_Create : Form
    {
        int amountAccs = 0;
        String[] Accounts = { };
        public Admin_Create()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form change = this;
            change.Close();
            Form console = new Admin_Console();
            console.Show();
        }

        private void Admin_Create_Load(object sender, EventArgs e)
        {
            GetAllAccounts();
            
            foreach(String account in Accounts)
            {
                amountAccs++;
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

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            AdminAccount obj = new AdminAccount
            {
                Id = (amountAccs + 1).ToString(),
                Nome = txt1.Text,
                Email = txt2.Text,
                Password = txt3.Text,
                Enabled = 1
            };
            try
            {
                string link = String.Format("http://localhost:50766/api/admin/newadmin/");

                WebRequest request = WebRequest.Create(link);
                request.Method = "POST";
                request.ContentType = "application/json";
                HttpWebResponse response = null;
                string result = JsonConvert.SerializeObject(obj);

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
                    MessageBox.Show("User Criado com sucesso");
                    amountAccs++;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar conta" + ex.Message + "\n" + ex.StackTrace);
                return;
            }

        }
                
        
    }
}
