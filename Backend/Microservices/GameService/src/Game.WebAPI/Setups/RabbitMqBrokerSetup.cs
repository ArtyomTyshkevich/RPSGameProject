using Game.Application.Interfaces.Buses;
using Game.Data.Consumers;
using MassTransit;
using Profile.DAL.Events;

namespace Game.WebAPI.Setups
{
    public static class RabbitMqBrokerSetup
    {
        public static void ConfigureBrokerMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit<IBrokerBus>(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["BrokerSettings:Host"], h =>
                    {
                        h.Username(configuration["BrokerSettings:Username"]!);
                        h.Password(configuration["BrokerSettings:Password"]!);
                    });

                    cfg.ReceiveEndpoint("UserDeletedQueue_GameService", e =>
                    {
                        e.ConfigureConsumer<UserDeleteConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("UserUpdatedQueue_GameService", e =>
                    {
                        e.ConfigureConsumer<UserUpdateConsumer>(context);

                        e.Bind("UserExchange", x =>
                        {
                            x.RoutingKey = "user.updated";
                        });
                    });


                    cfg.ClearSerialization();
                    cfg.UseRawJsonSerializer();
                });

                x.AddConsumer<UserDeleteConsumer>();
                x.AddConsumer<UserUpdateConsumer>();
            });
        }
    }
}