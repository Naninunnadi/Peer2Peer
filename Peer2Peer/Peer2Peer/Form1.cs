using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peer2Peer.utilities;

namespace Peer2Peer
{
    public partial class Form_main : Form
    {
        public Form_main()
        {
            InitializeComponent();
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
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form_main_Load(object sender, EventArgs e)
        {

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
    }
}
