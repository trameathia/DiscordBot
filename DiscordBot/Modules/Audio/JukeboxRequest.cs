using Discord.WebSocket;

namespace DiscordBot.Modules.Audio
{
	public sealed class JukeboxRequest
	{
		public SocketUser User { get; set; }
		public string VideoId { get; set; }
	}
}