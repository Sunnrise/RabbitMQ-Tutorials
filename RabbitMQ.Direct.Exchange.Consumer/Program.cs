using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.
channel.ExchangeDeclare(exchange: "direct-exchange-rabbit", type: ExchangeType.Direct);
//2.
string queueName = channel.QueueDeclare().QueueName;

//3.
channel.QueueBind(
    queue: queueName,
    exchange: "direct-exchange-rabbit",
    routingKey: "direct-MQ");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName, 
    autoAck: true, 
    consumer: consumer);
consumer.Received += (sender, eventArgs) =>
{
   string message = Encoding.UTF8.GetString(eventArgs.Body.Span);
    Console.WriteLine(message);
};
Console.Read();

//1. Declare exchange same as publisher
//2. Declare queue for consumer
//3. Bind queue with exchange