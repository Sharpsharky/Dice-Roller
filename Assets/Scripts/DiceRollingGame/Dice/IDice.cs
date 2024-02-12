namespace DiceRollingGame.Dice
{
    using System;
    using UnityEngine;

    public interface IDice
    {
        public void Initialize();
        public void StartWaitingForRest();
        
        public bool IsSelectable { get; set; }
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public Collider Collider { get; }
        
        public event Action<IDice, int> OnFinishedMoving;
    }
}