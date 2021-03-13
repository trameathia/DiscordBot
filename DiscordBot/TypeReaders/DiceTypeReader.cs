using Discord.Commands;
using DiscordBot.Modules.DiceRolling;
using System;
using System.Threading.Tasks;

namespace DiscordBot.TypeReaders
{
    public class DiceTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context,string input, IServiceProvider services)
        {
            if (Dice.TryParse(input, out Dice result))
                return Task.FromResult(TypeReaderResult.FromSuccess(result));

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a dice set!"));
        }

    }
}
