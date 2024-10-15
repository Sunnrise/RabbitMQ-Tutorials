using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(configure=>
{
    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://vbqelytc:MbF26kun1-5hNwzQ9Q8OQ--HckTNQuZu@shark.rmq.cloudamqp.com/vbqelytc");
    }); 

    builder.Services.AddHostedService<PublishMessageService>(provider=>
    {
        using IServiceScope scope = provider.CreateScope();
        IPublishEndpoint publishEndpoint= scope.ServiceProvider.GetService<IPublishEndpoint>();
        return new(publishEndpoint);
    });
});

var host = builder.Build();
host.Run();
