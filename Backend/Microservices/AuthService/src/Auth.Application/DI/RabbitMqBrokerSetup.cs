﻿using Auth.BLL.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BLL.DI
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

                    cfg.ReceiveEndpoint("UserDeletedQueue_AuthService", e =>
                    {
                        e.ConfigureConsumer<UserDeleteConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("UserUpdatedQueue_AuthService", e =>
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