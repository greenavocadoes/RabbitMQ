using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultipleChats
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChatUI.Form1 frm1 = new ChatUI.Form1();
            frm1.Text = "Dr Mochi";
            frm1.Chatter = "Dr Mochi";
            frm1.Show();

            ChatUI.Form1 frm2 = new ChatUI.Form1();
            frm2.Text = "Dr Yousef";
            frm2.Chatter = "Dr Yousef";
            frm2.Show();
            this.Hide();
        }
    }
}
