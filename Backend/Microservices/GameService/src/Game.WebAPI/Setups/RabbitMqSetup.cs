using Chat.Data.Consumers;
using MassTransit;

 namespace Game.WebAPI.Setups
{
    public static class RabbitMqSetup
    {
        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<GameSirchConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMqSettings:Host"], c =>
                    {
                        c.Username(configuration["RabbitMqSettings:Username"]!);
                        c.Password(configuration["RabbitMqSettings:Password"]!);
                    });

                    cfg.ReceiveEndpoint(configuration["RabbitMqSettings:QueueName"]!, e =>
                    {
                        e.ConfigureConsumer<GameSirchConsumer>(context);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}