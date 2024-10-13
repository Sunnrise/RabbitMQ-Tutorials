using System.Text;
using RabbitMQ.Client;


//Connection initialize
ConnectionFactory factory = new();
factory.Uri= new("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");


//Active connection and open channel
using IConnection connection =factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Queue Creation
channel.QueueDeclare(queue: "example-queue", exclusive: false);


//Send message to queue
//RabbitMQ accept messages as byte type. So we have to convert data to byte. 

for(int i=0;i<100;i++)
{   
    await Task.Delay(500);
    byte[] message=Encoding.UTF8.GetBytes("Helloworld "+i);
    channel.BasicPublish(exchange:"", routingKey:"example-queue", body: message);
}
Console.Read();