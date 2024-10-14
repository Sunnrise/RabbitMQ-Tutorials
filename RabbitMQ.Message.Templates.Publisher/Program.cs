using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Connection initialize
ConnectionFactory factory = new();
factory.Uri = new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P, Point to Point design
//string queueName = "Example-p2p-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false, 
//    exclusive: false, 
//    autoDelete: false);
//byte[]message= Encoding.UTF8.GetBytes("Hello from P2p!");
//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message);
#endregion

#region Pub/Sub, Publish/Subscribe design 
//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//byte[] message = Encoding.UTF8.GetBytes("Hello from Pub/Sub!");

//channel.BasicPublish(
//    exchange: exchangeName,
//    routingKey: string.Empty,
//    body: message);
#endregion

#region Work Queues design
//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for(int i = 0; i < 100; i++)
//{
//    await Task.Delay(1000);
//    byte[] message = Encoding.UTF8.GetBytes($"Hello from Work Queues! {i}");

//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message);
//}
#endregion

#region Request/Response design
string requestQueueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Create request message and sending
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for(int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("hello"+i);

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion

#region Listening response queue
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, eventArgs) =>
{
    if(eventArgs.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($" Response : {Encoding.UTF8.GetString(eventArgs.Body.Span)}");
    }
};
#endregion
#endregion

Console.Read();

