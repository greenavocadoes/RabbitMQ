using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MessagePublishing;
using Consumer;

namespace ChatUI
{
    public partial class Form1 : Form
    {
        private MessagePublishing.ExchangePublisher queue;
        private ExchangeSubscriber consumer;
        private const String HOST_NAME = "ec2-54-213-74-83.us-west-2.compute.amazonaws.com";
        // Use a publicly known DNS name here. Ask Ankit if you need one.
        // private const String HOST_NAME = "";
        private const int NODE = 5672;
        private const String EXCH_NAME = "chatExch";
        delegate void SetTextCallback(string text);
        private String _chatter;
        public Form1()
        {
            InitializeComponent();
        }

        public String Chatter {
            get {
                return _chatter;
            }

            set {
                _chatter = value;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            queue = new MessagePublishing.ExchangePublisher(HOST_NAME, EXCH_NAME, Chatter);

            // Subscribe to the queue
            //create the consumer
            consumer = new ExchangeSubscriber(HOST_NAME, EXCH_NAME, Chatter);

            //listen for message events
            consumer.onMessageReceived += handleMessage;

            //start consuming
            consumer.StartListening();
        }

        private void handleMessage(byte[] message)
        {
            if (message != null && message.Count() > 0)
            {
                String msg = Encoding.UTF8.GetString(message);
                AddMessage(msg);
            }
        }

        private void AddMessage(String msg)
        {
            if (this.textBox1.InvokeRequired)
			{	
				SetTextCallback d = new SetTextCallback(AddMessage);
				this.Invoke(d, new object[] { msg });
			}
			else
			{
                listBox1.Items.Add(msg);
			}
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBox1.Text))
                queue.Send(String.Format("{0}: {1}", Chatter, textBox1.Text));

            textBox1.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            consumer.Dispose();
            queue.Dispose();
            Application.Exit();
        }


    }
}
