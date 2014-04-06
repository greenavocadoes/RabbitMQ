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
using MessageConsuming;

namespace MessageClient
{
    public partial class frmChat : Form
    {
        private MessagePublishing.RabbitMQExchangePublisher publisher;
        private RabbitMQExchangeSubscriber consumer;
        private const String HOST_NAME = "ec2-54-213-74-83.us-west-2.compute.amazonaws.com";
        // Use a publicly known DNS name here. Ask Ankit if you need one.
        // private const String HOST_NAME = "";
        private const int NODE = 5672;
        private const String EXCH_NAME = "chatExch";
        delegate void SetTextCallback(string text);
        private String _chatUser;
        public frmChat()
        {
            InitializeComponent();
        }

        public String ChatUser {
            get {
                return _chatUser;
            }

            set {
                _chatUser = value;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            publisher = new MessagePublishing.RabbitMQExchangePublisher(HOST_NAME, EXCH_NAME, ChatUser);

            // Subscribe to the publisher
            //create the consumer
            consumer = new RabbitMQExchangeSubscriber(HOST_NAME, EXCH_NAME, ChatUser);

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
                publisher.Send(String.Format("{0}: {1}", ChatUser, textBox1.Text));

            textBox1.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            publisher.Dispose();
            Application.Exit();
        }


    }
}
