using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
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
            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false, cunsomer);

     

            cunsomer.Received += (object sender, BasicDeliverEventArgs e ) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj: " + message);

                //File.AppendAllText("log-critical.txt", message  + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
