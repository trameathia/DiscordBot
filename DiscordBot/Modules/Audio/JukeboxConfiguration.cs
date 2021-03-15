using System;

namespace DiscordBot.Modules.Audio
{
	public sealed class JukeboxConfiguration : ModuleComponentConfiguration
	{
		public int QueueSizeMax { get; set; }
		public TimeSpan ChannelLeaveTimeout { get; set; }
	}
}