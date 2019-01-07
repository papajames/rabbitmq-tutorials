using System;
using System.Text;
using RabbitMQ.Client;

namespace HelloWorld.Producer
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

                var message = $"Hello World! ({DateTime.Now:yyyy-MM-dd HH-mm-ss})";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
                Console.WriteLine("[x] Sent {0}", message);
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
