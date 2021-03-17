using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules.TrashCommands
{
    public sealed class Trash : ModuleComponent<ModuleComponentConfiguration, ModuleResult>
    {
        private bool _sheSaid;
        public Trash(ModuleComponentConfiguration config) : base(config) { }

        public bool Begin() => _sheSaid = true;
        public bool End() => _sheSaid = false;
        public async Task Said(SocketMessage socketMessage, DiscordSocketClient _DiscordClient, CommandHandler commandHandler)
        {
            if (socketMessage is not SocketUserMessage userMessage || userMessage.Author.Id == _DiscordClient.CurrentUser.Id)
                return;
            else if (!_sheSaid)
                return;
            else if (userMessage.Content.Equals($"{commandHandler.CommandPrefix}finish"))
                return;

            SocketCommandContext commandContext = new(_DiscordClient, userMessage);
            await commandContext.Channel.SendMessageAsync("That's what she said!");
        }
    }
}
