using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules.DiceRolling
{
    public class Dice
    {
        private readonly int _rolls;
        private readonly int _sides;
        public Dice(int rolls, int sides) => (_rolls, _sides) = (rolls, sides);
        public static bool TryParse(string input, out Dice dice)
        {
            dice = null;
            if (!IsValidRoll(input))
                return false;
            var temp = input.Split('d');
            dice = new Dice(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]));
            return true;
        }
        private static bool IsValidRoll(string roll) => Regex.IsMatch(roll, @"^\d*d\d*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public List<int> Roll()
        {
            List<int> rolls = new List<int>();
            for(int rollCount = 0; rollCount < _rolls; rollCount++)
            {
                rolls.Add(Singleton<Random>.Instance.Next(1, _sides + 1));
            }
            return rolls;
        }
        public override string ToString()
        {
            return $"{_rolls}d{_sides}";
        }
    }
}
