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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static List<string> statusList = new List<string>();

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var thread = new Thread(Request.doRequest);
            thread.Start();
            //Request.doRequest();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var thread = new Thread(TcpListenerServer.test);
            thread.Start();
        }
        //testime funktsioone
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine(FilterQuery.getAllParametersFromGetRequest("GET /?searchfile?var1=test1&var2=test2 HTTP/1.1"));
            Console.WriteLine(FilterQuery.getOperationFromGetRequest("GET /?searchfile?var1=test1&var2=test2 HTTP/1.1"));
            Console.WriteLine(FilterQuery.getUrlParameterValues("GET /?searchfile?var1=test1&var2=test2 HTTP/1.1"));
        }

    }
}