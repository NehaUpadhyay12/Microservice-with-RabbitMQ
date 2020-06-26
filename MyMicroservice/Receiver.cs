using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyMicroservice
{
    public class Receiver : BackgroundService
    {
        IModel _channel;
        IConnection _connection;
        string _queueName;
        public Receiver()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port= 5672, UserName="guest", Password="guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = "FirstQueue";
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }
    }
}
