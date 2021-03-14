using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules.DiceRolling
{
    [Name("Dice")]
    [Summary("Do dice shit")]
    public class DiceModule : ModuleBase<SocketCommandContext>
    {
        public DiceModule() { }

        [Command("roll")]
        [Summary("Roll dice in format xdx e.g. 2d6")]
        public async Task Roll(params Dice[] input) {
            DiceSet diceSet = new DiceSet(input);
            List<(Dice Dice, List<int> Results)> rolls = diceSet.Roll().ToList();
            
            Embed results = new EmbedBuilder
            {
                Color = new Color(230, 0, 126),
                Title = $"Total: {rolls.Select(r => r.Results.Sum() + r.Dice.getAdd()).Sum()}",
                Fields = rolls.Select(r => new EmbedFieldBuilder { Name = $"{r.Dice}: {r.Results.Sum() + r.Dice.getAdd()}" , Value = $"{{ {string.Join(", ", r.Results)} }}", IsInline = false }).ToList()
            }.Build();

            await ReplyAsync(embed: results);
        }
    }
}