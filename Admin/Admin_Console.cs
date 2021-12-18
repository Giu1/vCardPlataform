using Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using vCardPlatform.Models;
using vCardPlatformAPI.Models;
using Excel_Lib;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace AdminConsole
{
    public partial class Admin_Console : Form
    {
        //MqttClient broker;
        string[] Entities = { "Bank 1"};
        string[] EntIp = { };
        String[] curBank = { };
        int setvalueindex = -1;
        int index = -1;
        string[] topics = { "All" };
        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        MqttClient broker;
        String[] FilterTypes = { "By Bank", "By Transfer Type", "By Time", "By Balance" };
        String[] FilterTransfer = { "Credito","Debito" };
        String[] FilterTime = { "1 Week", "1 month", "6 months", "1 Year" };
        String[] FilterBalance = { "Most Money", "Least Money" };
        String localhost = "127.0.0.1";
        AdminAccount user = Global.CurrentUser;
        //Change String
        public string changeID { get; set; }

        public Admin_Console()
        {
            InitializeComponent();
            listBox1.Items.AddRange(Entities);
            comboBoxFiltro.DataSource = FilterTypes;
           
        }

        private void button16_Click(object sender, EventArgs e) //Btn Logout
        {
            //broker.Disconnect();
            //base.OnClosed(e);
            Form login = new LoginForm();
            login.Show();
            Form consolehere = this;
            consolehere.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //ConnectMosquitto();
            //RequestAllOperations();

        }
        private void ConnectMosquitto()
        {
             broker = new MqttClient(localhost);
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
             }
        }

        private void broker_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RequestAllOperations()
        {
            String[] s = { "Feed" };
            broker.Subscribe(s, qosLevels);
            MessageBox.Show("Subscribed to Feed Succefully");
            GetFeed();
            // TO DO
        }

      
        private void button17_Click(object sender, EventArgs e)
        {
            broker.Disconnect();
            base.OnClosed(e);
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)  //Btn Ping Bank
        {
            // Ping API by ID / or Ping All  Get
            // Get Bank Values for Setting   Get
            index = listBox1.SelectedIndex;
            string link = EntIp[index];
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

                curBank = (String[])serializer.Deserialize(strResul, typeof(String[]));
                MessageBox.Show("Sucesso\n Banco ID: "+curBank[0]+" Credito Maximo = "+curBank[1]+ "Debito Maximo = "+curBank[2] );
                textBox1.Text = curBank[1];
                textBox2.Text = curBank[2];
                setvalueindex = index;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao Pingar Banco: "+Entities[index]+"\n Verificar End Point\n" + ex.Message + "\n" + ex.StackTrace);

                return;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //emitir pagamento incluindo bank side

            UpdateBankValues(setvalueindex);
            // Set Value of Banks Post

        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Get da API com Parametros de Procura / Routes Diferentes para filtros inicais diferentes maybe
            // Mosquitto connects and publishes to either new channel or same channel with cleaned UI
            // Clean and Write on Console Field
        }

        private void button5_Click(object sender, EventArgs e)  // Btn Change 1
        {
            changeID = "1";
            Change(changeID);
        }
        private void Change(String x)  // Func Change
        {
            Form change = new ChangeXForm(x);
            change.Show();
            Form console = this;
            console.Close();
        }

        private void AdminConsole_Load(object sender, EventArgs e)
        {
            label3.Text = "Welcome : " + user.Nome;
            List<String> list = new List<String>();
            list.Add("https://localhost:44360/bank_1/bank/");
            EntIp = list.ToArray();
            ConnSubMq();


        }

        private void button6_Click(object sender, EventArgs e)  // Btn Change 2
        {
            changeID = "2";
            Change(changeID);
        }

        private void button4_Click(object sender, EventArgs e)  // Btn Change 3
        {
            changeID = "3";
            Change(changeID);
        }

        private void button2_Click(object sender, EventArgs e)  // Btn Change 4
        {
            changeID = "4";
            Change(changeID);
        }

        private void button3_Click(object sender, EventArgs e)  // Btn Change 5
        {
            changeID = "5";
            Change(changeID);
        }

        private void button7_Click(object sender, EventArgs e)  // Btn Create Admin
        {
            // Creation Form 
            Form console = this;
            console.Close();
            Form create = new Admin_Create();
            create.Show();
        }

        private void button9_Click(object sender, EventArgs e)  // Btn Change 6
        {
            changeID = "6";
            Change(changeID);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)     // Btn Filtar 1º Tipo
        {
            String choice = comboBoxFiltro.SelectedItem as String;
            switch (choice)
            {
                case "By Bank":
                    comboBoxParametros.DataSource = Entities;
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

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)      // Btn adicionar banco
        {
            if (textBox3.Text != "")
            {
                if(textBox4.Text != "")
                {
                    List<String> list = new List<String>();
                    List<String> ends = new List<String>();
                    list = Entities.ToList<string>();
                    ends = EntIp.ToList<string>();
                    string newBank = textBox3.Text;
                    string newEnd = "http://localhost:44360/" + textBox4.Text + "/";
                    // Fazer checks de qualidade de data TO DO

                    list.Add(newBank);
                    ends.Add(newEnd);

                    Entities = list.ToArray();
                    EntIp = ends.ToArray();
                    MessageBox.Show("Sucesso A Adicionar Banco");
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(Entities);
                }
                else { MessageBox.Show("Erro : Insira EndPoint Para Banco"); }
            }
            else { MessageBox.Show("Erro : Insira Nome Para Banco"); }
           
        }

        private void UpdateBankValues(int index)
        {
            if (index>=0)
            {
                try
                {
                    string link = EntIp[index] + "update/";
                    curBank[1] = textBox1.Text;
                    curBank[2] = textBox2.Text;
                    
                    WebRequest request = WebRequest.Create(link);
                    request.Method = "PUT";
                    request.ContentType = "application/json";
                    HttpWebResponse response = null;
                    string result = JsonConvert.SerializeObject(curBank);

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
                        MessageBox.Show("Alterações realizadas com sucesso");
                        textBox1.Text = curBank[1];
                        textBox2.Text = curBank[2];
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao modificar valores de banco\n" + ex.Message + "\n" + ex.StackTrace);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Selecione e Pinge um Banco Primeiro");
                return;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            listBox2.Text = "";
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void GetFeed()
        {
            string link = String.Format("http://localhost:50766/api/admin/feed/");
            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "GET";
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: Ao buscar o feed" + ex.Message + ex.StackTrace);

                return;
            }

        }
        
        private void ConnSubMq()
        {
            broker = new MqttClient("127.0.0.1");
            try
            {

                byte var =  broker.Connect(Guid.NewGuid().ToString());
                if (!broker.IsConnected)
                {
                    Console.WriteLine("Error connecting to message broker...");
                    return;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Verifique a disponibilidade do broker");
            }

            broker.MqttMsgPublishReceived += Broker_MqttMsgPublishReceived;
            broker.MqttMsgSubscribed += Broker_MqttMsgSubscribed;

            broker.Subscribe(topics, qosLevels);
            

        }

        private void Broker_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            //MessageBox.Show("Acabou de subscrever os topicos");
        }

        private void Broker_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);
            this.Invoke((MethodInvoker)delegate
            {
            listBox2.Items.Add(msg); });
            Console.WriteLine("You");
            
                     }

        private void button14_Click(object sender, EventArgs e)
        {
            XML xml = new XML();
            xml.logs = listBox2.Text;
            string filename = "LogX.xml";
            xml.Save(filename);
            String fullPath = Path.GetFullPath(filename);

            Console.WriteLine(fullPath);
            MessageBox.Show("Ficheiro Criado Com Sucesso");
           
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
            var json = JsonConvert.SerializeObject(listBox2.Text);
            Console.WriteLine(json);
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            JsonLayoutOptions options = new JsonLayoutOptions();

            JsonUtility.ImportData(json, worksheet.Cells, 0, 0, options);
            workbook.Save("LogE.xlsx");
            string fileName = "Log.xlsx";
            String fullPath = Path.GetFullPath(fileName);

            Console.WriteLine(fullPath);
        }
    }

}
