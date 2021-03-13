using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions.Equivalency;
using FluentAssertions;
using DiscordBot.Modules.DiceRolling;
using System.Collections.Generic;

namespace DiscordBot.Tests
{
    [TestClass]
    public class DiceRolling
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Assert.IsTrue(false);
            bool b = false;
            b.Should().Be(true);
        }

        public void TestDiceSet()
        {
            DiceSet diceSet = new()
            {
                new Dice(2, 6),
                new Dice(5, 20)
            };
            IEnumerable<(Dice Dice, List<int> Results)> rolls = diceSet.Roll();

            Assert.IsTrue(isJordanAwesome());
        }
        private bool isJordanAwesome() => true;
    }
}
