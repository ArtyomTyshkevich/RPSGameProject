using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MassTransit;

namespace Game.WebAPI.Setups
{
    public static class RabbitMqSetup
    {
        public static void ConfigureBrokerMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {

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
                    cfg.ConfigureEndpoints(context);
                });

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();
            });
        }
    }
}
