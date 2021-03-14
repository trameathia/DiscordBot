using Discord.Audio;
using System.Collections.Concurrent;

namespace DiscordBot.Modules.Audio
{
	public sealed class JukeboxConnection
	{
		public ulong ServerId { get; set; }
		public IAudioClient AudioClient { get; set; }
		public ulong? CurrentChannelId { get; set; }
		public ConcurrentQueue<JukeboxRequest> Queue { get; set; } = new ConcurrentQueue<JukeboxRequest>();
		public JukeboxRequest CurrentRequest { get; set; }
	}
}