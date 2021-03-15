using Discord.WebSocket;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DiscordBot.Modules.Audio
{
	public sealed class JukeboxRequest
	{
		public SocketUser User { get; set; }
		public string VideoId { get; set; }
	}
}