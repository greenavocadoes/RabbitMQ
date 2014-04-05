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


        private ConnectionFactory factory = new ConnectionFactory();

        private IConnection connection;
        private IModel channel;
        private string exch;
        
        public QueueSend(String host, String exchName, String chatter)
        {
            factory.HostName = host;            
            factory.ClientProperties.Add("Chatter", chatter);
            connection = factory.CreateConnection();
            
            
            
            channel = connection.CreateModel();
            
            
            IBasicProperties props = channel.CreateBasicProperties();            
            channel.ExchangeDeclare(exchName, "fanout");
            exch = exchName;
        }

        public void Send(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exch, "", null, body);
        }

   
   

        public void Dispose()
        {
            if (channel != null)
                channel.Close();
            if (connection != null)
                connection.Close();
           
        }
    }
}
