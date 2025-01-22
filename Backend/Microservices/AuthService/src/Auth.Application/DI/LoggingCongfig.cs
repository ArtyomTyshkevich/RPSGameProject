using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Auth.BLL.DI
{
    public static class LoggingCongfig
    {
        public static IHostBuilder ConfigureLogs(this IHostBuilder @this, IConfiguration configuration)
        {
            if (configuration["Logstash:Uri"] is not string logstashUri)
            {
                throw new KeyNotFoundException("There is no Logstash:Uri in configuration");
            }
            @this.UseSerilog((context, config) =>
            {
                config.WriteTo.Http(logstashUri);
            });
            return @this;
        }
    }
}