using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Modules.DiceRolling
{
    public class DiceSet : IEnumerable<Dice>
    {
        private readonly List<Dice> _dice;
        public DiceSet(params Dice[] dice) => _dice = new List<Dice>(dice);
        public DiceSet(IEnumerable<Dice> dice) => _dice = dice.ToList();
        public void Add(Dice d) => _dice.Add(d);
        public static bool TryParse(string input, out DiceSet diceSet)
        {
            diceSet = null;
            string[] temp = input.Split(' ');
            if (temp.Length == 0)
                return false;
            diceSet = new DiceSet();
            foreach(string s in temp)
            {
                if (!Dice.TryParse(s, out Dice dice))
                    return false;
                diceSet.Add(dice);
            }
            return true;
        }

        public IEnumerator<Dice> GetEnumerator()
        {
            return _dice.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<(Dice Dice, List<int> Results)> Roll() => _dice.Select(dice => (dice, dice.Roll()));
    }
}
