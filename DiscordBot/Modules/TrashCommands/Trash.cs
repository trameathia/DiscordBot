using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DiscordBot.Modules.TrashCommands
{
	public sealed class Trash : ModuleComponent<ModuleComponentConfiguration, ModuleResult>
    {
        private bool _sheSaid;
        public Trash(ModuleComponentConfiguration config) : base(config) { }

        public bool Begin() => _sheSaid = true;
        public bool End() => _sheSaid = false;
        public async Task Said(SocketMessage socketMessage, DiscordSocketClient _DiscordClient, IOptionsMonitor<DiscordBotConfiguration> configuration)
        {
            if (socketMessage is not SocketUserMessage userMessage || userMessage.Author.Id == _DiscordClient.CurrentUser.Id)
                return;
            else if (!_sheSaid)
                return;
            else if (userMessage.Content.Equals($"{configuration.CurrentValue.Prefix}finish"))
                return;

            SocketCommandContext commandContext = new(_DiscordClient, userMessage);
            await commandContext.Channel.SendMessageAsync("That's what she said!");
        }
    }
}
