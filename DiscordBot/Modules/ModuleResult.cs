using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Modules
{
	public class ModuleResult
	{
		public List<ModuleResultMessage> Messages { get; set; } = new List<ModuleResultMessage>();

		public bool HasInformation => GetInformationMessages().Any();
		public bool HasWarnings => GetWarningMessages().Any();
		public bool HasErrors => GetErrorMessages().Any();

		public IEnumerable<ModuleResultMessage> GetInformationMessages() => Messages.Where(message => message.Type == ResultMessageType.Information);
		public IEnumerable<ModuleResultMessage> GetWarningMessages() => Messages.Where(message => message.Type == ResultMessageType.Warning);
		public IEnumerable<ModuleResultMessage> GetErrorMessages() => Messages.Where(message => message.Type == ResultMessageType.Error);

		public void AddInformationMessage(string content) => Messages.Add(new ModuleResultMessage { Type = ResultMessageType.Information, Content = content });
		public void AddWarningMessage(string content) => Messages.Add(new ModuleResultMessage { Type = ResultMessageType.Warning, Content = content });
		public void AddErrorMessage(string content) => Messages.Add(new ModuleResultMessage { Type = ResultMessageType.Error, Content = content });

		public static ModuleResult FromInformation(string content) => FromInformation<ModuleResult>(content);

		public static T FromInformation<T>(string content)
			where T : ModuleResult, new()
		{
			T result = new();
			result.AddInformationMessage(content);
			return result;
		}

		public static ModuleResult FromWarning(string content) => FromWarning<ModuleResult>(content);

		public static T FromWarning<T>(string content)
			where T : ModuleResult, new()
		{
			T result = new();
			result.AddWarningMessage(content);
			return result;
		}

		public static ModuleResult FromError(string content) => FromError<ModuleResult>(content);

		public static T FromError<T>(string content)
			where T : ModuleResult, new()
		{
			T result = new();
			result.AddErrorMessage(content);
			return result;
		}
	}

	public class ModuleResult<T> : ModuleResult
	{
		public T Result { get; set; }

		public static ModuleResult<T> FromResult(T result) => new() { Result = result };
	}
}