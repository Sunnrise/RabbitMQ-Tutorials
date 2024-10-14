using RabbitMQ.Client;
using System.Text;

//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare("fanout-exchange-rabbit", ExchangeType.Fanout);

for(int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message= Encoding.UTF8.GetBytes($"Zühremcim {i}");
    channel.BasicPublish(
        exchange:"fanout-exchange-rabbit", 
        routingKey:string.Empty,
        body: message);
}
Console.Read();

