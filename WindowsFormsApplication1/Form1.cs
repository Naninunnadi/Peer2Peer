﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartRequest(4);
 
        }

        public void StartRequest(int ttl)
        {
            var places = Utilities.getIpsAndPorts();
            foreach (var keyValuePair in places)
            {
                var requestModel = Utilities.ParseRequest("/searchfile?name=" + textBox1.Text + "&sendip=" + Utilities.LocalIPAddress() + "&sendport=" + Utilities.LocalPort() + "&ttl="+ttl+"&id=wqeqwe23&noask=" + Utilities.LocalIPAddress());
                var request = new Request(requestModel, keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
                Thread worker = new Thread(request.doRequest);
                worker.IsBackground = true;
                worker.SetApartmentState(System.Threading.ApartmentState.STA);
                worker.Name = "STARTERParseRequestsAndSendRequestsTTL";
                worker.Start();
            }
            
	    }

        public void StartRequest2()
        {
            Thread worker = new Thread(TcpListenerServer.test);
            worker.IsBackground = true;
            worker.SetApartmentState(System.Threading.ApartmentState.STA);
            worker.Name = "TCPLISTENERTHREAD";
            worker.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartRequest2();
        }


        public List<KeyValuePair<IPAddress, int>> getIpsAndPorts()
        {
            List<KeyValuePair<IPAddress, int>> clientList = new List<KeyValuePair<IPAddress, int>>();
            string[] clients = System.IO.File.ReadAllLines("IP&Port.txt");

            foreach (var client in clients)
            {
                IPAddress iP = null;
                int port = 0;
                string[] pieces = client.Split(null);
                if (pieces.FirstOrDefault() != null)
                    iP = IPAddress.Parse(pieces.FirstOrDefault());
                if (pieces.LastOrDefault() != null)
                    int.TryParse(pieces.LastOrDefault(), out port);
                if (iP != null && port != 0)
                    clientList.Add(new KeyValuePair<IPAddress, int>(iP, port));
            }
            return clientList;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var myIp = Utilities.LocalIPAddress();
            var myPort = Utilities.LocalPort();
            //foreach (var user in getIpsAndPorts())
            //{
            //    HttpProcessor.Connect(user.Key.ToString(), "http://" + user.Key + ":" + user.Value + "/searchfile?name=" + SearchBox.Text + "&sendip=" + myIp + "&sendport=" + myPort + "&ttl=5&id=wqeqwe23&noask=" + myIp, user.Value);

            //}

        }
        public string getTextBoxValue()
        {
            return textBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void StartRequest3()
        {
            Thread worker = new Thread(PostRequestHandler.postRequest);
            worker.IsBackground = true;
            worker.SetApartmentState(System.Threading.ApartmentState.STA);
            worker.Name = "PostRequestThreadHE";
            worker.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartRequest3();
        }
    }
}