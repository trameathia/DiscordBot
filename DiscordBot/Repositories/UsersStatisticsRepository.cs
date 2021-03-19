using DiscordBot.Data;
using DiscordBot.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DiscordBot.Repositories
{
	public sealed class UsersStatisticsRepository
	{
		private readonly IServiceScopeFactory _ScopeFactory;

		public UsersStatisticsRepository(IServiceScopeFactory scopeFactory)
		{
			_ScopeFactory = scopeFactory;
		}

		public async Task<UserStatistics> AddUserStatistics(UserStatistics userStatistics)
		{
			using IServiceScope scope = _ScopeFactory.CreateScope();
			DiscordBotDbContext context = scope.ServiceProvider.GetRequiredService<DiscordBotDbContext>();

			await context.UsersStatistics.AddAsync(userStatistics);
			return userStatistics;
		}

		public async Task<UserStatistics> GetUserStatistics(ulong guildId, string username)
		{
			using IServiceScope scope = _ScopeFactory.CreateScope();
			DiscordBotDbContext context = scope.ServiceProvider.GetRequiredService<DiscordBotDbContext>();

			return await context.UsersStatistics.FindAsync(guildId, username);
		}

		public UserStatistics UpdateUserStatistics(UserStatistics userStatistics)
		{
			using IServiceScope scope = _ScopeFactory.CreateScope();
			DiscordBotDbContext context = scope.ServiceProvider.GetRequiredService<DiscordBotDbContext>();

			context.UsersStatistics.Update(userStatistics);
			return userStatistics;
		}

		public void RemoveUserStatistics(UserStatistics userStatistics)
		{
			using IServiceScope scope = _ScopeFactory.CreateScope();
			DiscordBotDbContext context = scope.ServiceProvider.GetRequiredService<DiscordBotDbContext>();

			context.UsersStatistics.Remove(userStatistics);
		}
	}
}