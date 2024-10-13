using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Queue Creation
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);


//Get message from queue
//RabbitMQ carry messages as byte type. So we have to convert byte to actual type
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue",autoAck:false,consumer: consumer);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1,global: false);
consumer.Received += (sender, e) =>
{//e.body give us whole message as byte
 //e.body.Span or e.body.ToArray() can be used to convert byte to actual type
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    channel.BasicAck(deliveryTag: e.DeliveryTag,multiple: false);
};

Console.Read();