using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.DiceRolling
{
    [Name("Dice")]
    [Summary("Do dice shit")]
    public class DiceModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _rand;
        public DiceModule(Random rand) => _rand = rand;

        [Command("roll")]
        [Summary("Roll dice in format xdx e.g. 2d6")]
        public async Task Roll(params Dice[] input) {
            DiceSet diceSet = new DiceSet(input);
            List<(Dice Dice, List<int> Results)> rolls = diceSet.Roll().ToList();

            Embed results = new EmbedBuilder
            {
                Color = new Color(114, 137, 218),
                Title = $"Total: {rolls.Select(r => r.Results.Sum()).Sum()}",
                Description = $"{{ {string.Join(", ", rolls.Select(r => r.Results.Sum()))} }}",
                Fields = rolls.Select(r => new EmbedFieldBuilder { Name = $"{r.Dice}: {r.Results.Sum()}" , Value = $"{{ {string.Join(", ", r.Results)} }}", IsInline = false }).ToList()
            }.Build();

            await ReplyAsync(embed: results);
        }
    }
}