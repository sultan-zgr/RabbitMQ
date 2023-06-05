using RabbitMQ.Client;
using System;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace RabbitMQ.publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warnin = 3,
        Info = 4
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vzivcfpz:oPFuu8queJQGxr5mCRx1k91shAf5gx4c@chimpanzee.rmq.cloudamqp.com/vzivcfpz");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue -{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct",routeKey, null);
            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);

                string message = $"log-type: {log}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey=$"route-{log}";
                channel.BasicPublish("logs-direct", "", null, messageBody);
                Console.WriteLine($"Log gönderilmiştir: {message}");

            });
            Console.ReadLine();


        }
    }
}
