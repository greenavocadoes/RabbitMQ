using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;

namespace MessageConsuming
{
    public class RabbitMQExchangeSubscriber :IDisposable
    {
        protected IModel Model;
        protected IConnection Connection;
        protected string QueueName;

        protected bool isListening;

        // used to pass messages back to UI for processing
        public delegate void onReceiveMessage(byte[] message);
        public event onReceiveMessage onMessageReceived;

        public RabbitMQExchangeSubscriber(string hostName, string exchName, string chatUser)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = hostName;
            connectionFactory.ClientProperties.Add("ChatUser", chatUser);
            
            Connection = connectionFactory.CreateConnection();
            Model = Connection.CreateModel();
            
            QueueName = Model.QueueDeclare(String.Format("{0}_queue",chatUser),false,true, true, null)  ;
            Model.ExchangeDeclare(exchName, "fanout");
            Model.QueueBind(QueueName, exchName, "");
                        

        }

        //internal delegate to run the queue consumer on a seperate thread
        private delegate void ConsumeDelegate();

        public void StartListening()
        {
            isListening = true;
            ConsumeDelegate c = new ConsumeDelegate(Consume);
            c.BeginInvoke(null, null);
        }

        public void Consume()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(Model);
            String consumerTag = Model.BasicConsume(QueueName, true, consumer);
            while (isListening)
            {
                try
                {
                    RabbitMQ.Client.Events.BasicDeliverEventArgs e = (RabbitMQ.Client.Events.BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    IBasicProperties props = e.BasicProperties;
                    byte[] body = e.Body;
                    
                    onMessageReceived(body);
                    //Model.BasicAck(e.DeliveryTag, false);
                }
                catch (EndOfStreamException ex) {            	 
            	    throw;
            	}
                catch (OperationInterruptedException ex)
                {   
                    throw;
                }
            }
        }

        public void Dispose()
        {
            isListening = false;
            if (Model != null)
                Model.Close();
            if (Connection != null)
                Connection.Close();
        
        }
    }
}
