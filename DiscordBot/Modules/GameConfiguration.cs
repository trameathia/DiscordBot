using System;

namespace DiscordBot.Modules
{
	public class GameConfiguration
	{
		public TimeSpan TimerDelay { get; set; }
		public TimeSpan TimerInterval { get; set; }
		public int TimerTicksMax { get; set; }
	}
}