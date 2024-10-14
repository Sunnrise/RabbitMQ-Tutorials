using RabbitMQ.Client;
using System.Text;

//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare("topic-exchange-rabbit", ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Message {i}");
    Console.WriteLine("Type a topic: ");
    string topic = Console.ReadLine();
    channel.BasicPublish(
        exchange: "topic-exchange-rabbit",
        routingKey: topic,
        body: message);
}
Console.Read();

