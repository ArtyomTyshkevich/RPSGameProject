using Game.Data.HangfireJobs;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Chat.Data.Extensions
{
    public static class AddHangfireExtension
    {
        public static IServiceCollection AddHangfireConfig(this IServiceCollection @this, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("gameredis");

            @this.AddHangfire(globalConfiguration =>
            {
                globalConfiguration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseRedisStorage(redisConnectionString);
            });
            @this.AddHangfireServer();

            return @this;
        }

        public static void SetupJobs(this IServiceProvider @this)
        {
            using var scope = @this.CreateScope();

            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            ConfigureRecurringJobs(recurringJobManager, scope.ServiceProvider);
        }

        private static void ConfigureRecurringJobs(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            var jobId = "clean-rooms-in-preparation";

            Expression<Func<Task>> action = () => serviceProvider
                .GetRequiredService<CleanRoomsJob>()
                .ExecuteAsync(CancellationToken.None);

            recurringJobManager.AddOrUpdate(jobId, action, "*/1 * * * *");
        }
    }
}
