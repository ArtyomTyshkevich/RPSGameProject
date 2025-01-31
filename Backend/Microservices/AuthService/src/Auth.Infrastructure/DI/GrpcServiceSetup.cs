using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserGrpcService;

namespace Auth.BLL.DI
{
    public static class GrpcServiceSetup
    {
        public static void ConfigureGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            var gameServiceUrl = configuration["GrpcSettings:GameServiceUrl"];
            var chatServiceUrl = configuration["GrpcSettings:ChatServiceUrl"];
            var profileServiceUrl = configuration["GrpcSettings:ProfileServiceUrl"];

            services.AddGrpcClient<GameServiceGRPC.GameServiceGRPCClient>(options =>
            {
                options.Address = new Uri(gameServiceUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            services.AddGrpcClient<ChatServiceGRPC.ChatServiceGRPCClient>(options =>
            {
                options.Address = new Uri(chatServiceUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            services.AddGrpcClient<ProfileServiceGRPC.ProfileServiceGRPCClient>(options =>
            {
                options.Address = new Uri(profileServiceUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }
    }
}
