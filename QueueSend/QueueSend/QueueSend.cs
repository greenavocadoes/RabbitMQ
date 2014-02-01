using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace QueueSend
{
    public class QueueSend : IDisposable
    {
      
        private ConnectionFactory factory = new ConnectionFactory() {HostName = "localhost"};
        private IConnection connection;
        private IModel channel;
        
        public QueueSend()
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare("hello", false, false, false, null);

        }

        public void Send(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "hello", null, body);
        }



        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}
