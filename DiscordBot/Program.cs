using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class Program
    {
        public static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilder, services) =>
                {
                    Core.ConfigureServices(hostBuilder.Configuration, services);
                    services.AddHostedService<Core>();
                })
                .Build()
                .RunAsync();
    }
}