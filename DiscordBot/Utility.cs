using System;
using System.Linq;

namespace DiscordBot
{
	public static class Utility
	{
		public static string FormatTimeSpan(TimeSpan timespan) =>
			string.Join(
				" and ",
				new (int Amount, string Verbiage)[]
				{
					(timespan.Days, "day"),
					(timespan.Hours, "hour"),
					(timespan.Minutes, "minute"),
					(timespan.Seconds, "second")
				}
					.Where(timespanComponent => timespanComponent.Amount > 0)
					.Take(2)
					.Select(timespanComponent => $"{timespanComponent.Amount} {timespanComponent.Verbiage}{(timespanComponent.Amount > 1 ? "s" : string.Empty)}"));
	}
}