using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bend.Util;

namespace P2P
{
	public partial class Form1 : Form
	{
		public string Name { get; set; }
		public IList<KeyValuePair<IPAddress, int>> IpsAndPorts { get; set; }
		//public IPAddress SendIp { get; set; }
		//public int SendPort { get; set; }
		//public int Ttl { get; set; }
		//public int Id { get; set; }
		//public int Noask { get; set; }
		//public UdpClient receivingClient { get; set; }
		//public UdpClient sendingClient { get; set; }
		//public Thread receivingThread { get; set; }


		public Form1(String[] args)
		{
			
			InitializeComponent();
				HttpServer httpServer;
			if (args.GetLength(0) > 0)
			{
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]));
			}
			else
			{
				httpServer = new MyHttpServer(8080);
			}
			Thread thread = new Thread(new ThreadStart(httpServer.listen));
			thread.Start();
			

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
			foreach (var user in getIpsAndPorts())
			{

                HttpProcessor.Connect(user.Key.ToString(), "http://" + user.Key + ":" + user.Value + "/searchfile?name=" + SearchBox.Text + "&sendip=" + myIp + "&sendport=" + myPort + "&ttl=5&id=wqeqwe23&noask=" + myIp, user.Value);
			}
			
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }

	}
}
