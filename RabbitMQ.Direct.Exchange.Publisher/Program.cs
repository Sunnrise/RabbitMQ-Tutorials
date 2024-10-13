using System.Text;
using RabbitMQ.Client;


//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"direct-exchange-rabbit", type: ExchangeType.Direct);

while(true)
{
    Console.WriteLine("Enter message to send to RabbitMQ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "direct-exchange-rabbit",
        routingKey: "direct-MQ",                     
        body: byteMessage);
    Console.WriteLine("Message sent to RabbitMQ");
}
Console.Read();