using System;

namespace DiscordBot.Modules.Audio
{
	public sealed class QueueResult
	{
		public string Title { get; set; }
		public TimeSpan Duration { get; set; }
	}
}