namespace DiceRollingGame.Dice
{
    using Roller;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class DiceManager : MonoBehaviour, IInitializable
    {
        private List<IDice> dice;
        private IRoller roller;
        private Coroutine rollRoutine;

        private const float RollFrequency = 0.3f;

        [Inject]
        public void Construct(List<IDice> dice, IRoller roller)
        {
            this.dice = new List<IDice>(dice);
            this.roller = roller;
        }

        public void Initialize()
        {
            foreach (var die in dice)
            {
                die.Initialize();
            }
        }

        public void RandomRoll()
        {
            if (dice == null || dice.Count == 0)
                return;

            if (rollRoutine != null)
            {
                StopCoroutine(rollRoutine);
                rollRoutine = null;
            }

            rollRoutine = StartCoroutine(RandomRollSequence());
        }

        private IEnumerator RandomRollSequence()
        {
            foreach (var die in dice)
            {
                roller.RandomRoll(die);
                yield return new WaitForSeconds(RollFrequency);
            }
        }

    }
}