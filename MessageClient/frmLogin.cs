using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetaDataService;

namespace MessageClient
{
    public partial class frmLogin : Form
    {
        
        private ChatBrokerService brokerSerice; 
        public frmLogin()
        {
            InitializeComponent();
            brokerSerice = new ChatBrokerService("http://ec2-54-213-74-83.us-west-2.compute.amazonaws.com", "guest", "guest");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            button1.Enabled = !String.IsNullOrEmpty(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (brokerSerice.isNameAvailable(textBox1.Text))
            {
                frmChat frm = new frmChat();
                frm.Text = String.Format("{0} says...", textBox1.Text);
                frm.ChatUser = textBox1.Text;
                frm.Show();
                this.Visible = false;
            }
            else
            {
                label2.Visible = true;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !String.IsNullOrEmpty(textBox1.Text);
            label2.Visible = false;
        }
    }
}
