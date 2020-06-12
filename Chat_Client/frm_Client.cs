using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;


namespace Chat_Client
{
    public partial class frm_Client : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        private TcpClient client;
        public StreamReader str;
        public StreamWriter stw;
        public string receive;
        public string TexttoSend;

        public frm_Client()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach(IPAddress address in localIP)
            {
                if(address.AddressFamily==AddressFamily.InterNetwork)
                {
                    tbIPY.Text = address.ToString();
                }
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(tbPortY.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            str = new StreamReader(client.GetStream());
            stw = new StreamWriter(client.GetStream());
            stw.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IPEnd = new IPEndPoint(IPAddress.Parse(tbIPYF.Text), int.Parse(tbPortYF.Text));
            try
            {
                client.Connect(IPEnd);
                if(client.Connected)
                {
                    tbChatScreen.AppendText("Connect to another client..."+"\n");
                    str = new StreamReader(client.GetStream());
                    stw = new StreamWriter(client.GetStream());
                    stw.AutoFlush = true;
                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(client.Connected)
            {
                try
                {
                    receive = str.ReadLine();
                    this.tbChatScreen.Invoke(new MethodInvoker(delegate ()
                    {
                        tbChatScreen.AppendText("You: " + receive + "\n");
                    }));
                    receive = "";
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if(client.Connected)
            {
                stw.WriteLine(TexttoSend);
                this.tbChatScreen.Invoke(new MethodInvoker(delegate ()
                {
                    tbChatScreen.AppendText("Me: " + TexttoSend + "\n");
                }));
            }
            else
            {
                MessageBox.Show("Sending failed");
            }
            backgroundWorker2.CancelAsync();

        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if(tbMess.Text!="")
            {
                TexttoSend = tbMess.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            tbMess.Text = " ";
        }

        private void frm_Client_Load(object sender, EventArgs e)
        {
            tbUserName.Text = frm_Login.showUser;
        }
    }
}
