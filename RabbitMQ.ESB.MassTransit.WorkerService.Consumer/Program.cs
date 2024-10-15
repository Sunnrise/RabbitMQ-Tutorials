using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<ExampleMessageConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");

        cfg.ReceiveEndpoint("example-message-queue", e =>
        {
            e.ConfigureConsumer<ExampleMessageConsumer>(context);
        });
    });

    
});
var host = builder.Build();
host.Run();

