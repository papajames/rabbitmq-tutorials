using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HelloWorld.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq.papajames.lab",
                UserName = "admin",
                Password = "pass.123"
            };
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var counter = 0;

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var number = Interlocked.Increment(ref counter);

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    Thread.Sleep(1000);
                    Console.WriteLine(" [{1}] Received {0}", message, number);
                };
                channel.BasicConsume(
                    queue: "hello",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
