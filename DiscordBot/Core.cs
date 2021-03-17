using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Modules.NumberGuessingGame;
using DiscordBot.Modules.Audio;
using DiscordBot.Modules.TrashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Core
    {
        public IConfigurationRoot Configuration { get; }

        public Core(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("_config.json")
                .Build();
        }

        public static async Task RunAsync(string[] args) => await new Core(args).RunAsync();

        public async Task RunAsync()
        {
			ServiceCollection services = new();
            ConfigureServices(services);

			ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<LoggingService>();
            serviceProvider.GetRequiredService<CommandHandler>();

            await serviceProvider.GetRequiredService<CoreService>().StartAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }))
            .AddSingleton<CommandHandler>()
            .AddSingleton<CoreService>()
            .AddSingleton<LoggingService>()
            .AddNumberGuessingGame()
            .AddJukebox()
            .AddTrash()
            .AddSingleton(Configuration);
        }
    }
}