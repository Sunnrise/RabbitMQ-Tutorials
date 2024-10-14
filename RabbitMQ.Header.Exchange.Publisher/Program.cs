using RabbitMQ.Client;
using System.Text;

//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange-rabbit", ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Message {i}");
    Console.WriteLine("Type a Header Value: ");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();

    basicProperties.Headers = new Dictionary<string, object>
    {
        { "no", value }
    };

    channel.BasicPublish(
        exchange: "header-exchange-rabbit",
        routingKey: string.Empty,
        body: message,
        basicProperties: basicProperties);
}
Console.Read();

