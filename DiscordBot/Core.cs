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
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Reflection;
using DiscordBot.Modules.DiceRolling;
using DiscordBot.TypeReaders;
using Microsoft.Extensions.Options;
using DiscordBot.Data;
using DiscordBot.Repositories;

namespace DiscordBot
{
    public class Core : IHostedService
    {
        private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;
        private readonly IServiceProvider _ServiceProvider;
        private readonly DiscordSocketClient _DiscordClient;
        private readonly CommandService _CommandService;

        public Core(IOptionsMonitor<DiscordBotConfiguration> configuration, IServiceProvider serviceProvider, DiscordSocketClient discordClient, CommandService commandService)
        {
            _Configuration = configuration;
            _ServiceProvider = serviceProvider;
            _DiscordClient = discordClient;
            _CommandService = commandService;

            serviceProvider.GetRequiredService<CommandHandler>();
        }

        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<DiscordBotConfiguration>(configuration);

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
            .AddSingleton<LoggingService>()
            .AddSingleton<CommandHandler>()
            .AddDbContext<DiscordBotDbContext>()
            .AddSingleton<UsersStatisticsRepository>()
            .AddNumberGuessingGame()
            .AddJukebox()
            .AddTrash();
        }

		public async Task StartAsync(CancellationToken cancellationToken)
		{
            if (string.IsNullOrWhiteSpace(_Configuration.CurrentValue.DiscordToken))
            {
                throw new Exception("Please enter your bot's authorization token into the `appsettings.json` file.");
            }

            await _DiscordClient.LoginAsync(TokenType.Bot, _Configuration.CurrentValue.DiscordToken);
            await _DiscordClient.StartAsync();

            _CommandService.AddTypeReader(typeof(Dice), new DiceTypeReader());

            await _CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), _ServiceProvider);
        }

		public Task StopAsync(CancellationToken cancellationToken)
		{
            return Task.CompletedTask;
        }
	}
}