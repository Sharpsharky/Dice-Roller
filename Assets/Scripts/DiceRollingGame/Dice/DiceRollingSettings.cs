namespace DiceRollingGame.Dice
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Dice Rolling Settings")]
    public class DiceRollingSettings : ScriptableObject
    {
        [SerializeField] private float rollingHeight = 3;
        [SerializeField] private float flyToHandTime = 0.1f;
        [SerializeField] private float maxDiceVelocity = 40;
        [SerializeField] private float diceVelocityCoefficient = 20;
        [SerializeField] private float velocityCoefficient = 10;
        [SerializeField] private float velocityExponent = 5;
        [SerializeField] private float minSpeedToToss = 8;
        
        public float RollingHeight => rollingHeight;
        public float FlyToHandTime => flyToHandTime;
        public float MaxDiceVelocity => maxDiceVelocity;
        public float DiceVelocityCoefficient => diceVelocityCoefficient;
        public float VelocityCoefficient => velocityCoefficient;
        public float VelocityExponent => velocityExponent;
        public float MinSpeedToToss => minSpeedToToss;
    }
}