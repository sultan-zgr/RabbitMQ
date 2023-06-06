using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace RabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vzivcfpz:oPFuu8queJQGxr5mCRx1k91shAf5gx4c@chimpanzee.rmq.cloudamqp.com/vzivcfpz");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();



            channel.BasicQos(0, 1, false);
            var cunsomer = new EventingBasicConsumer(channel);
            var queueName = channel.QueueDeclare().QueueName;

            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            headers.Add("x-match", "all");



            channel.QueueBind(queueName, "header-exchange", String.Empty,headers);

            channel.BasicConsume(queueName, false, cunsomer);
            Console.WriteLine("Loglar dinleniyor...");
     

            cunsomer.Received += (object sender, BasicDeliverEventArgs e ) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj: " + message);


                channel.BasicAck(e.DeliveryTag, false);
            };

            //Console.ReadLine();
        }
    }
}
