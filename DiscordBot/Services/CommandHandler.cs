using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
	public sealed class CommandHandler
    {
        private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;
        private readonly IServiceProvider _Services;
        private readonly DiscordSocketClient _DiscordClient;
        private readonly CommandService _CommandService;

        public CommandHandler(IOptionsMonitor<DiscordBotConfiguration> configuration, IServiceProvider services, DiscordSocketClient discordClient, CommandService commandService)
        {
            _Configuration = configuration;
            _Services = services;
            _DiscordClient = discordClient;
            _CommandService = commandService;

            _DiscordClient.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage userMessage || userMessage.Author.Id == _DiscordClient.CurrentUser.Id)
            {
                return;
            }

            SocketCommandContext commandContext = new(_DiscordClient, userMessage);
            int prefixIndex = 0;

            if (userMessage.HasStringPrefix(_Configuration.CurrentValue.Prefix, ref prefixIndex) || userMessage.HasMentionPrefix(_DiscordClient.CurrentUser, ref prefixIndex))
            {
                IResult commandResult = await _CommandService.ExecuteAsync(commandContext, prefixIndex, _Services);

                if (!commandResult.IsSuccess)
                {
                    await commandContext.Channel.SendMessageAsync(commandResult.ToString());
                }
            }
        }
    }
}