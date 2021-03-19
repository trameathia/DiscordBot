using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordBot.Data
{
	public sealed class DiscordBotDbContext : DbContext
	{
		private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;

		public DiscordBotDbContext(IOptionsMonitor<DiscordBotConfiguration> configuration)
		{
			_Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite($"Data Source={_Configuration.CurrentValue.DatabaseFilePath}");
		}
	}
}