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
    exchange: "header-exchange-rabbit",
    type: ExchangeType.Headers);

Console.WriteLine("type a header value: ");
string value = Console.ReadLine();
string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
    queue: queueName,
    exchange: "header-exchange-rabbit",
    routingKey: string.Empty,
    new Dictionary<string, object>
    {
        { "no", value }
    });

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
