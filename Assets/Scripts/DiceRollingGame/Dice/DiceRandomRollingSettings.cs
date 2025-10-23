namespace DiceRollingGame.Dice
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Dice Random Rolling Settings")]
    public class DiceRandomRollingSettings : ScriptableObject
    {
        [SerializeField] private float randomDirectionOffset = 0.5f;
        [SerializeField] private float randomAngleOffset = 1f;
        [SerializeField] private float minRandomVelocity = 8f;
        [SerializeField] private float maxRandomVelocity = 12f;
        [SerializeField] private float minRandomRotation = 8f;
        [SerializeField] private float maxRandomRotation = 12f;

        public float RandomDirectionOffset => randomDirectionOffset;
        public float RandomAngleOffset => randomAngleOffset;
        public float MinRandomVelocity => minRandomVelocity;
        public float MaxRandomVelocity => maxRandomVelocity;
        public float MinRandomRotation => minRandomRotation;
        public float MaxRandomRotation => maxRandomRotation;
    }
}