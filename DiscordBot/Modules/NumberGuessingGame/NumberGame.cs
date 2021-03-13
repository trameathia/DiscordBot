using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Modules.NumberGuessingGame
{
	public sealed class NumberGame : Game<NumberGameConfiguration, NumberGameResult>
	{
		private readonly Dictionary<string, int> _Guesses;
		private int _MagicNumber;

		public NumberGame(NumberGameConfiguration configuration) : base(configuration)
		{
			_Guesses = new Dictionary<string, int>();
		}

		protected override NumberGameResult BuildResult()
		{
			return new NumberGameResult
			{
				MagicNumber = _MagicNumber,
				WinnerName = _Guesses.FirstOrDefault(guess => guess.Value == _MagicNumber).Key
			};
		}

		protected override void OnStart()
		{
			_Guesses.Clear();
			_MagicNumber = Singleton<Random>.Instance.Next(1, Configuration.MagicNumberMax + 1);
		}

		public bool RegisterGuess(string playerId, int guess)
		{
			if (!IsRunning || string.IsNullOrWhiteSpace(playerId) || _Guesses.ContainsKey(playerId) || _Guesses.Values.OfType<int>().Contains(guess))
			{
				return false;
			}

			_Guesses.Add(playerId, guess);
			return true;
		}
	}
}