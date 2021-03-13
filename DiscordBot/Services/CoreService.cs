using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Modules.DiceRolling;
using DiscordBot.TypeReaders;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    class CoreService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        public CoreService(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;
        }

        public async Task StartAsync()
        {
            string discordToken = _config["discordToken"];
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            _commands.AddTypeReader(typeof(Dice), new DiceTypeReader());

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
