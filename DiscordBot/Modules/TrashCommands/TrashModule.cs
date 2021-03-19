using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DiscordBot.Modules.TrashCommands
{
	[Name("Trash")]
    [Summary("Random Trash")]
    public class TrashModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;
        private readonly Trash _Trash;

        public TrashModule(IOptionsMonitor<DiscordBotConfiguration> configuration, Trash trash)
        {
            _Configuration = configuration;
            _Trash = trash;
        }

        private async Task Said(SocketMessage socketMessage) => await _Trash.Said(socketMessage, this.Context.Client, _Configuration);

        [Command("she")]
        [Summary("That's what she said!")]
        public async Task She()
        {
            Context.Client.MessageReceived += Said;
            await ReplyAsync(message: (_Trash.Begin()) ? "Here she comes!" : "Not this time bois!");
        }

        [Command("finish")]
        [Summary("She done!")]
        public async Task Finish()
        {
            Context.Client.MessageReceived -= Said;
            await ReplyAsync(message: (!_Trash.End()) ? "She's done!" : "She's not finished yet!");
        }
    }
}
