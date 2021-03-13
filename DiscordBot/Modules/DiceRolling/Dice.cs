using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules.DiceRolling
{
    public class Dice
    {
        private readonly int _rolls;
        private readonly int _sides;
        private readonly int _add;
        public Dice(int rolls, int sides, int add) => (_rolls, _sides, _add) = (rolls, sides, add);
        public static bool TryParse(string input, out Dice dice)
        {
            dice = null;
            int add = 0;
            if (!IsValidRoll(input))
                return false;
            var temp = input.Split('d');
            var parts = Regex.Split(temp[1], @"[()+-]");
            if (parts.Length > 1)
                add = Convert.ToInt32(parts[1]);
            if (temp[1].Contains('-'))
                add = add * -1;
            dice = new Dice(Convert.ToInt32(temp[0]), Convert.ToInt32(parts[0]), add);
            return true;
        }
        private static bool IsValidRoll(string roll) => Regex.IsMatch(roll, @"^\d*d\d*[()+-]*\d*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public int Add() => _add;
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
            return $"{_rolls}d{_sides}{(_add != 0 ? (_add > 0 ? "+"+_add : _add) : string.Empty)}";
        }
    }
}
