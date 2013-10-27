using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peer2Peer.core;
using Peer2Peer.utilities;

namespace Peer2Peer
{
    public partial class Form_main : Form
    {
        public string Queries { get; set; }

        public Form_main()
        {
            InitializeComponent();
            //System.Net.ServicePointManager.MaxServicePointIdleTime = 10000;
            this.AutoSize = true;
            var IpsSnfPorts = Factory.getIpsAndPorts();
            listView1.View = View.Details;
            listView1.Columns.Add("IP", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Port", 100, HorizontalAlignment.Left);
            foreach (var keyValuePair in IpsSnfPorts)
            {
                ListViewItem item = new ListViewItem();
                item.Text = keyValuePair.Key.ToString();
                item.SubItems.Add(keyValuePair.Value);
                listView1.Items.Add(item);
            }
            label3.Text = Factory.LocalIPAddress();
            label5.Text = Factory.LocalPort();
            flowLayoutPanel1.AutoScroll = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (flowLayoutPanel1.Controls.Count > 0)
            {
                var controltoremove = flowLayoutPanel1.Controls[0];
                flowLayoutPanel1.Controls.Remove(controltoremove);
                controltoremove.Dispose();
            }
            StartRequest(4);
        }

        public void StartRequest(int ttl)
        {
            var places = utilities.Factory.getIpsAndPorts();
            foreach (var keyValuePair in places)
            {
                var requestModel = utilities.Factory.ParseRequest("/searchfile?name=" + textBox1.Text.Trim() + "&sendip=" + utilities.Factory.LocalIPAddress() + "&sendport=" + utilities.Factory.LocalPort() + "&ttl=" + ttl + "&id=wqeqwe23&noask=" + utilities.Factory.LocalIPAddress());
                var request = new GetRequestHandler(requestModel, keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
                Thread worker = new Thread(request.doRequest);
                worker.IsBackground = true;
                worker.SetApartmentState(System.Threading.ApartmentState.STA);
                worker.Name = "STARTERParseRequestsAndSendRequestsTTL";
                worker.Start();

            }

        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            StartTcpListenerThread();
        }

        public void StartTcpListenerThread()
        {
            var tcpListener = new TcpServer(flowLayoutPanel1);
            
            Thread worker = new Thread(tcpListener.TcpServerListen);
            worker.IsBackground = true;
            worker.SetApartmentState(System.Threading.ApartmentState.STA);
            worker.Name = "TCPLISTENERTHREAD";
            
            worker.Start();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //private void GetFile(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    var ipandPort = btn.Tag.ToString().Split(':');
        //    Thread worker = new Thread(new DownloadManager(ipandPort[0], ipandPort[1], btn.Text).doRequestForGetFile);
        //    worker.IsBackground = true;
        //    worker.SetApartmentState(System.Threading.ApartmentState.STA);
        //    worker.Name = "REQUESTFILETHREAD";
        //    worker.Start();


        //    DownloadManager.ListenForFile(@"c:\wazaa\" + btn.Text);


        //}
    }
}
