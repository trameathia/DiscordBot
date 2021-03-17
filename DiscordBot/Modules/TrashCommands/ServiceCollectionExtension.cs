using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Modules.TrashCommands
{
    public static class ServiceCollectionExtension
    {
		public static IServiceCollection AddTrash(this IServiceCollection services)
		{
			return services.AddSingleton(serviceProvider => new Trash(new ModuleComponentConfiguration()));
		}
	}
}
