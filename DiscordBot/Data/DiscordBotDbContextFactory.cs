using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiscordBot.Data
{
	/// <summary>
	/// This class is only used during the development process to generate a new db file via Add-Migration/Update-Database.
	/// </summary>
	public sealed class DiscordBotDbContextFactory : IDesignTimeDbContextFactory<DiscordBotDbContext>
	{
		public DiscordBotDbContext CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<DiscordBotDbContext> optionsBuilder = new();
			optionsBuilder.UseSqlite($"Data Source=DiscordBot.db");

			return new DiscordBotDbContext(optionsBuilder.Options, null);
		}
	}
}