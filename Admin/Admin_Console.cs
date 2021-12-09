using Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AdminConsole
{
    public partial class Admin_Console : Form
    {
        //MqttClient broker;
        string[] Entities = { "Bank 1", "Bank 2", "MBWay" };
        String[] FilterTypes = { "By Bank", "By Transfer Type", "By Time", "By Balance" };
        String[] FilterBank = { "Bank 1", "Bank 2", "MBWay " };
        String[] FilterTransfer = { "Withdraw", "Deposit", "Payment" };
        String[] FilterTime = { "1 Week", "1 month", "6 months", "1 Year" };
        String[] FilterBalance = { "Most Money", "Least Money" };
        String localhost = "127.0.0.1";

        //Change String
        public string changeID { get; set; }

        public Admin_Console()
        {
            InitializeComponent();
            listBox1.Items.AddRange(Entities);
            comboBoxFiltro.DataSource = FilterTypes;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form login = new LoginForm();
            login.Show();
            Form consolehere = this;
            consolehere.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //ConnectMosquitto();
            //RequestAllOperations();

        }
        private void ConnectMosquitto()
        {
            /* broker = new MqttClient(localhost);
             try
             {

                 broker.Connect(Guid.NewGuid().ToString());
                 if (!broker.IsConnected)
                 {
                     Console.WriteLine("Error connecting to message broker...");
                     return;
                 }
                 MessageBox.Show("Connected to message broker");

             }
             catch (Exception)
             {
                 MessageBox.Show("Verifique a disponibilidade do broker");
             }*/
        }
        private void RequestAllOperations()
        {
            // TO DO
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ping API by ID / or Ping All  Get
            // Get Bank Values for Setting   Get
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String choice = comboBoxFiltro.SelectedItem as String;
            switch (choice)
            {
                case "By Bank":
                    comboBoxParametros.DataSource = FilterBank;
                    break;
                case "By Transfer Type":
                    comboBoxParametros.DataSource = FilterTransfer;
                    break;
                case "By Time":
                    comboBoxParametros.DataSource = FilterTime;
                    break;
                case "By Balance":
                    comboBoxParametros.DataSource = FilterBalance;
                    break;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            // Set Value of Banks Post

        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Get da API com Parametros de Procura / Routes Diferentes para filtros inicais diferentes maybe
            // Mosquitto connects and publishes to either new channel or same channel with cleaned UI
            // Clean and Write on Console Field
        }

        private void button5_Click(object sender, EventArgs e)
        {
            changeID = "1";
            Change(changeID);
        }
        private void Change(String x)
        {
            Form change = new ChangeXForm(x);
            change.Show();
        }

        private void AdminConsole_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            changeID = "2";
            Change(changeID);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeID = "3";
            Change(changeID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeID = "4";
            Change(changeID);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changeID = "5";
            Change(changeID);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Creation Form 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            changeID = "6";
            Change(changeID);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
