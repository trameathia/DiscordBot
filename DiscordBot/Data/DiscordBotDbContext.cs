using DiscordBot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordBot.Data
{
	public sealed class DiscordBotDbContext : DbContext
	{
		private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;

		public DbSet<UserStatistics> UsersStatistics { get; set; }

		public DiscordBotDbContext(DbContextOptions<DiscordBotDbContext> options, IOptionsMonitor<DiscordBotConfiguration> configuration) : base(options)
		{
			_Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Configuration can be null in a development environment where we are running a command to generate a non-existent db file,
			// in which case the options builder will be configured via the DiscordBotDbContextFactory class.
			if (_Configuration != null)
			{
				optionsBuilder.UseSqlite($"Data Source={_Configuration.CurrentValue.DatabaseFilePath}");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserStatistics>()
				.HasIndex(userStatistics => userStatistics.Id)
				.IsUnique(true);
			modelBuilder.Entity<UserStatistics>()
				.HasIndex(userStatistics => new { userStatistics.GuildId, userStatistics.Username })
				.IsUnique(true);
		}
	}
}