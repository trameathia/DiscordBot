using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.Screamo
{
    [Name("Screamo")]
    [Summary("Screams and shit")]
    public class ScreamoModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _service;

        public ScreamoModule(AudioService service)
        {
            _service = service;
        }

        [Command("stalkme", RunMode = RunMode.Async)]
        public async Task StalkMe()
        {

            //var channel = (Context.User as IGuildUser)?.VoiceChannel;
            //if(channel == null) {
            //    await Context.Channel.SendMessageAsync("User must be in a voice channel");
            //    return;
            //}

            //var audoClient = await channel.ConnectAsync();

            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("disappear", RunMode = RunMode.Async)]
        public async Task Disappear()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("scream", RunMode = RunMode.Async)]
        public async Task Scream([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}
