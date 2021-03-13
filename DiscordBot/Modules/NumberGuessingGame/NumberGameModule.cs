using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Modules.NumberGuessingGame
{
	[Name("Number Guessing Game")]
	[Summary("Play the number guessing game")]
	public class NumberGameModule : ModuleBase<SocketCommandContext>
	{
		private readonly CommandHandler _CommandHandler;
		private readonly NumberGame _Game;

		public NumberGameModule(CommandHandler commandHandler, NumberGame game)
		{
			_CommandHandler = commandHandler;
			_Game = game;
		}

		private async Task OnGameEnd(NumberGameResult result)
		{
			await ReplyAsync(message: (string.IsNullOrEmpty(result.WinnerName) ?
				$"Game over! No one guessed the correct number." :
				$"{result.WinnerName} is the champion!") +
				$" (the number was {result.MagicNumber})");
		}

		[Command("numbergame")]
		[Summary("Starts the number guessing game")]
		public async Task StartGame() => await ReplyAsync(message: _Game.Start(OnGameEnd) ?
			$"I'm thinking of a number between 1 and {_Game.Configuration.MagicNumberMax}. Can you guess what it is? You have {_Game.Configuration.TimerDelay.TotalSeconds:N0} seconds. GO! (use the {_CommandHandler.CommandPrefix}guess command)" :
			"The number guessing game is already in progress!");

		[Command("guess")]
		[Summary("Guesses a number for the number guessing game")]
		public async Task GuessNumber([Summary("The number to guess (may not be a number that has already been guessed)")] int number)
		{
			if (!_Game.IsRunning)
			{
				await ReplyAsync(message: $"The game has not yet been started. Start it with the {_CommandHandler.CommandPrefix}numbergame command!");
				return;
			}

			if (number < 0 || number > 10)
			{
				await ReplyAsync(message: $"{Context.User.Mention} don't be a dummy, I said between 1 and {_Game.Configuration.MagicNumberMax}!");
				return;
			}

			if (!_Game.RegisterGuess(Context.User.Mention, number))
			{
				await ReplyAsync(message: $"{Context.User.Mention} you already guessed you silly goose!");
				return;
			}

			await ReplyAsync(message: $"{Context.User.Mention} guessed {number}.");
		}
	}
}