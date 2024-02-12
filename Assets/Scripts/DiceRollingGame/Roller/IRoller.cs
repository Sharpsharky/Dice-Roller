namespace DiceRollingGame.Roller
{
    using System;
    using Dice;
    using UnityEngine;

    public interface IRoller
    {
        void Initialize(Camera camera);
        void Grab(IDice dices);
        void ThrowCurrentTargetable();
        void RandomRoll(IDice dice);
        
        public event Action<IDice> OnDiceTossed;
    }
}