namespace DiceRollingGame.Dice
{
    using System.Collections.Generic;
    using Input;
    using Managers;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public class DiceManager : SerializedMonoBehaviour, IManager
    {
        [OdinSerialize] private List<IDice> dice;
        [OdinSerialize] private IRoller diceRollingController;

        public void Initialize()
        {
            foreach (var die in dice)
            {
                die.Initialize();
            }
        }

        public void RandomRoll()
        {
            if(dice.Count != 0)
                diceRollingController.RandomRoll(dice[0]);
        }
    }
}