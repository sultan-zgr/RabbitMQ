using RabbitMQ.Client;
using System;
using System.Collections.Generic;
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

            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;

            channel.BasicPublish("header-exchange", string.Empty, properties,
            Encoding.UTF8.GetBytes("header mesajı"));
            Console.WriteLine("gönderildi");
            Console.ReadLine();


        }
    }
}
