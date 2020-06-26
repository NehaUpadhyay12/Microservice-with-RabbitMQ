using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldService
{
    public class Sender: ISender
    {
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672, UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queueName = "FirstQueue";
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                }
            }
        }
    }
}
