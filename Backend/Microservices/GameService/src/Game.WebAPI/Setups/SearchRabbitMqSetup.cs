using Game.Application.Interfaces.Buses;
using Game.Data.Consumers;
using MassTransit;

 namespace Game.WebAPI.Setups
{
    public static class SearchRabbitMqSetup
    {
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<ISearchRabbitMqBus>(x =>
            {
                x.AddConsumer<GameSearchConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqSettings:Host"], c =>
                    {
                        c.Username(configuration["RabbitMqSettings:Username"]!);
                        c.Password(configuration["RabbitMqSettings:Password"]!);
                    });

                    cfg.ReceiveEndpoint(configuration["RabbitMqSettings:QueueName"]!, e =>
                    {
                        e.ConfigureConsumer<GameSearchConsumer>(context);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}