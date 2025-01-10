using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserGrpcService; 

namespace Auth.BLL.DI
{
    public static class GrpcServiceSetup
    {
        public static void ConfigureGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            //var profileServiceUrl = configuration["GrpcSettings:ProfileServiceUrl"];
            var gameServiceUrl = configuration["GrpcSettings:GameServiceUrl"];
            var chatServiceUrl = configuration["GrpcSettings:ChatServiceUrl"];

            //services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
            //{
            //    options.Address = new Uri(profileServiceUrl);
            //});

            services.AddGrpcClient<GameService.GameServiceClient>(options =>
            {
                options.Address = new Uri(gameServiceUrl);
            });

            services.AddGrpcClient<ChatService.ChatServiceClient>(options =>
            {
                options.Address = new Uri(chatServiceUrl);
            });
        }
    }
}
