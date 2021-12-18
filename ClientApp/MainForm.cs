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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using vCardPlatform.Models;
using vCardPlatformAPI.Models;

namespace ClientApp
{
    public partial class MainForm : Form
    {

        private static User AuthUser { get; set; }
        MqttClient broker;
        string[] topics = { "All" };
        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE};
        RichTextBox log;
        int run = 0;
        public MainForm(User user)
        {
            InitializeComponent();
            AuthUser = user;
            LoadElements();
        }

        private void LoadElements()
        {
            log = this.richTextBox1;
            this.label1.Text = AuthUser.AccountOwner;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.label4.Text = AuthUser.Balance + " €";
            ConnSubMq();
            if (AuthUser.Photo != null)
            {
                this.pictureBox1.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(AuthUser.Photo)));
                

            }
            else
            {
                this.pictureBox1.Image = (Image)Properties.Resources.defautImage;
            }

            string linkSave = String.Format("http://localhost:50766/api/movimentos/user/"+AuthUser.Id);

            List<MovimentoBancario> registos;
           
            if(run == 0)
            {
                try
                {
                    WebRequest requestPassword = WebRequest.Create(linkSave);
                    requestPassword.Method = "GET";
                    HttpWebResponse responsePassword = null;

                    responsePassword = (HttpWebResponse)requestPassword.GetResponse();

                    String strResul = null;
                    using (Stream stream = responsePassword.GetResponseStream())
                    {

                        StreamReader reader = new StreamReader(stream);
                        strResul = reader.ReadToEnd();
                        reader.Close();
                    }

                    var serializer = new JavaScriptSerializer();

                    registos = (List<MovimentoBancario>)serializer.Deserialize(strResul, typeof(List<MovimentoBancario>));
                    //this.richTextBox1.Text = "";

                    foreach (var item in registos)
                    {
                        if (item.Type == TypeOfMoviment.Credito && item.IdSender== AuthUser.Id+"")
                        {
                            string add = "\nEnvias te " + item.Amount + " € a " + item.IdReceiver + "\n";
                            log.AppendText(add + "\r\n");

                        }
                        if (item.Type == TypeOfMoviment.Debito && item.IdReceiver == AuthUser.Id + "")
                        {
                            string add = "Recebes te " + item.Amount + " € de " + item.IdSender+"\n";
                            log.AppendText(add + "\r\n");
                        }
                    }

                }
                catch (Exception)
                {

                    this.richTextBox1.Text = "Erro";

                }
                run = 1;

            }
           
            //this.richTextBox1.Text = 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            ProfileForm f2 = new ProfileForm(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void ReloadUser()
        {
            string link = String.Format("http://localhost:50766/api/conta/" + AuthUser.Id);
            WebRequest requestPassword = WebRequest.Create(link);
            requestPassword.Method = "GET";
            HttpWebResponse responsePassword = null;

            responsePassword = (HttpWebResponse)requestPassword.GetResponse();

            String strResul = null;
            using (Stream stream = responsePassword.GetResponseStream())
            {

                StreamReader reader = new StreamReader(stream);
                strResul = reader.ReadToEnd();
                reader.Close();
            }

            var serializer = new JavaScriptSerializer();

            AuthUser = (User)serializer.Deserialize(strResul, typeof(User));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pagamento f2 = new Pagamento(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Depositar f2 = new Depositar(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Levantar f2 = new Levantar(AuthUser);
            if (f2.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Comprar f1 = new Comprar(AuthUser);
            if (f1.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            ReloadUser();
            LoadElements();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //TODO - check password serverside
            PasswordForm frm = new PasswordForm(AuthUser);
            if (frm.ShowDialog() != DialogResult.OK)
            {
                // The user canceled.
                this.Close();
            }

            //emitir pagamento incluindo bank side

            string link = String.Format("http://localhost:50766/api/conta/"+AuthUser.Id);

            MovimentoBancario registoNoBanco = new MovimentoBancario();

            try
            {
                WebRequest request = WebRequest.Create(link);
                request.Method = "DELETE";
                request.ContentType = "application/json";
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    char[] charsToTrim = { '"', '\'' };


                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseText = reader.ReadToEnd();
                        if (responseText != "\"Sucesso\"")
                        {
                            MessageBox.Show(responseText);
                        }
                        else
                        {
                            MessageBox.Show(responseText);
                            this.DialogResult = DialogResult.OK;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao emitir nota - emitir movimento bank side" + ex.Message + "\n" + ex.StackTrace);

                return;
            }

        }

        private void ConnSubMq()
        {
            broker = new MqttClient("127.0.0.1");
            try
            {

                byte var = broker.Connect(Guid.NewGuid().ToString());
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
            string[] topicInfo = { "n" + AuthUser.Id.ToString() };
            broker.Subscribe(topicInfo, qosLevels);


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
               
                log.AppendText(msg + "\r\n");
                //listBox2.Items.Add(msg);
            });
            Console.WriteLine("You");

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
