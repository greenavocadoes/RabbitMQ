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
using QueueSend;
using ChatConsumer;

namespace ChatUI
{
    public partial class Form1 : Form
    {
        private QueueSend.QueueSend queue;
        private Consumer consumer;
        private const String HOST_NAME = "localhost";
        private const String QUEUE_NAME = "hello";
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
            queue = new QueueSend.QueueSend(HOST_NAME, QUEUE_NAME);

            // Subscribe to the queue
            //create the consumer
            consumer = new Consumer(HOST_NAME, QUEUE_NAME);

            //listen for message events
            consumer.onMessageReceived += handleMessage;

            //start consuming
            consumer.StartConsuming();
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
                queue.Send(String.Format("{0}:{1}", Chatter, textBox1.Text));

            textBox1.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            queue.Dispose();
        }


    }
}
