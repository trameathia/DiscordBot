using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Data.Models
{
	public sealed class UserStatistics
	{
		[Required]
		public ulong Id { get; set; }
		[Required]
		public ulong GuildId { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public ulong MessagesSent { get; set; }
	}
}