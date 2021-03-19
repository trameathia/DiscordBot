using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
	[Name("Help")]
    [Summary("For Dummies")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly IOptionsMonitor<DiscordBotConfiguration> _Configuration;
        private readonly CommandService _CommandService;

        public HelpModule(IOptionsMonitor<DiscordBotConfiguration> configuration, CommandService commandService)
        {
            _Configuration = configuration;
            _CommandService = commandService;
        }

        [Command("help")]
        [Summary("Displays the list of commands")]
        public async Task HelpAsync()
        {
            EmbedBuilder embedBuilder = new()
            {
                Color = new Color(114, 137, 218),
                Description = "These are the commands you can use"
            };

            foreach (ModuleInfo module in _CommandService.Modules)
            {
                StringBuilder moduleDescription = new();

                foreach (CommandInfo command in module.Commands)
                {
                    PreconditionResult preconditionResult = await command.CheckPreconditionsAsync(Context);

                    if (preconditionResult.IsSuccess)
                    {
                        moduleDescription.Append($"{_Configuration.CurrentValue.Prefix}{command.Aliases[0]}");

                        if (command.Parameters.Count > 0)
                        {
                            moduleDescription.Append($" {string.Join(' ', command.Parameters.Select(parameter => $"<{parameter.Name}>"))}");
                        }

                        if (!string.IsNullOrWhiteSpace(command.Summary))
                        {
                            moduleDescription.Append($" - {command.Summary}");
                        }

                        moduleDescription.Append('\n');
                    }
                }

                if (moduleDescription.Length > 0)
                {
                    embedBuilder.AddField(field =>
                    {
                        field.Name = $"{module.Name} - {module.Summary}";
                        field.Value = moduleDescription.ToString();
                        field.IsInline = false;
                    });
                }
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }

        [Command("help")]
        [Summary("Displays information for a specific command")]
        public async Task HelpAsync([Summary("The command to display information for")] string command)
        {
            SearchResult searchResult = _CommandService.Search(Context, command);

            if (!searchResult.IsSuccess)
            {
                await ReplyAsync($"Unable to find a command like **{command}**");
                return;
            }

            EmbedBuilder embedBuilder = new()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (CommandMatch match in searchResult.Commands)
            {
                embedBuilder.AddField(field =>
                {
                    field.Name = $"{_Configuration.CurrentValue.Prefix}{match.Command.Aliases[0]}";
                    field.Value = $"{match.Command.Summary}";

                    if (match.Command.Parameters.Count > 0)
                    {
                        field.Name += $" {string.Join(' ', match.Command.Parameters.Select(parameter => $"<{parameter.Name}>"))}";
                        field.Value += $"\nParameters:";

                        foreach (ParameterInfo parameter in match.Command.Parameters)
                        {
                            field.Value += $"\n- {parameter.Name}";

                            if (!string.IsNullOrWhiteSpace(parameter.Summary))
                            {
                                field.Value += $": {parameter.Summary}";
                            }
                        }
                    }

                    field.IsInline = false;
                });
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}