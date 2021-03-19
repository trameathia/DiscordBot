using System.Text.Json.Serialization;

namespace DiscordBot
{
	public sealed class DiscordBotConfiguration
	{
		public string Prefix { get; set; }
		public string DiscordToken { get; set; }
	}
}