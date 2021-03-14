using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Modules.Audio
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddJukebox(this IServiceCollection services)
		{
			return services.AddSingleton(serviceProvider => new Jukebox(new JukeboxConfiguration
			{
				QueueSizeMax = 50
			}));
		}
	}
}