using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;
Console.WriteLine("Publisher");
string rabbitMQUri = "amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc";

string RequestQueueName = "request-Queue";
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);

});
await bus.StartAsync();

var request= bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{RequestQueueName}"));

int i = 1;
while (true)
{
   await Task.Delay(200);
   var response =await request.GetResponse<ResponseMessage>(new(){ MessageNo=i, Text=$"{i}.request"});
    Console.WriteLine($"Response received: {response.Message.Text}");
    i++;
}