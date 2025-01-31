using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.BLL.Consumers;

namespace Profile.BLL.DI
{
    public static class RabbitMqBrokerSetup
    {
        public static void ConfigureBrokerMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["BrokerSettings:Host"], h =>
                    {
                        h.Username(configuration["BrokerSettings:Username"]!);
                        h.Password(configuration["BrokerSettings:Password"]!);
                    });

                    cfg.ReceiveEndpoint("SaveGameResultQueue_ProfileService", e =>
                    {
                        e.ConfigureConsumer<SaveGameResultConsumer>(context);
                    });

                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                });

                x.AddConsumer<SaveGameResultConsumer>();
            });
        }
    }
}