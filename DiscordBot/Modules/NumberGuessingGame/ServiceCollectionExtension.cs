using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace DiscordBot.Modules.NumberGuessingGame
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddNumberGuessingGame(this IServiceCollection services)
		{
			return services.AddSingleton(serviceProvider => new NumberGame(new NumberGameConfiguration
			{
				TimerDelay = TimeSpan.FromSeconds(15.0),
				TimerInterval = Timeout.InfiniteTimeSpan,
				MagicNumberMax = 10
			}));
		}
	}
}