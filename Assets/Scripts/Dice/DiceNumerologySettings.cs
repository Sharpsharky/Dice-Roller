namespace DiceRollingGame.Dice
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Dice Numerology Settings")]
    public class DiceNumerologySettings : ScriptableObject
    {
        [SerializeField] private List<int> diceNumbers;
        
        public List<int> DiceNumbers => diceNumbers;
    }
}