using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "topic-exchange-rabbit",
    type: ExchangeType.Topic);

Console.WriteLine("type a topic: ");
string topic = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
    queue: queueName,
    exchange: "topic-exchange-rabbit",
    routingKey: topic);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine($"Received message: {message}");
};

Console.Read();
