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
    exchange: "fanout-exchange-rabbit",
    type: ExchangeType.Fanout);

Console.WriteLine("type a queue name");
string queueName = Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive: false);
channel.QueueBind(
    queue: queueName,
    exchange: "fanout-exchange-rabbit",
    routingKey: string.Empty);

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
