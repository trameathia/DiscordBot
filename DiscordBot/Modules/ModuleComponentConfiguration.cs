using System;

namespace DiscordBot.Modules
{
	public class ModuleComponentConfiguration
	{
		public TimeSpan TimerDelay { get; set; }
		public TimeSpan TimerInterval { get; set; }
		public int TimerTicksMax { get; set; }
		public bool StartOnLoad { get; set; }
	}
}