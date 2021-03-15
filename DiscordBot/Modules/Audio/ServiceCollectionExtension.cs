using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiscordBot.Modules.Audio
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddJukebox(this IServiceCollection services)
		{
			return services.AddSingleton(serviceProvider => new Jukebox(new JukeboxConfiguration
			{
				TimerDelay = TimeSpan.Zero,
				TimerInterval = TimeSpan.FromSeconds(1.0),
				TimerTicksMax = -1,
				StartOnLoad = true,
				QueueSizeMax = 50,
				ChannelLeaveTimeout = TimeSpan.FromSeconds(15.0)
			}));
		}
	}
}