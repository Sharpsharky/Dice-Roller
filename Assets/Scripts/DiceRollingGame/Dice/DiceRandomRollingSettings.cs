namespace DiceRollingGame.Dice
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/Dice Random Rolling Settings")]
    public class DiceRandomRollingSettings : ScriptableObject
    {
        [SerializeField] float randomDirectionOffset = 0.5f;
        [SerializeField] float randomAngleOffset = 1f;
        [SerializeField] float minRandomVelocity = 8f;
        [SerializeField] float maxRandomVelocity = 12f;
        [SerializeField] float minRandomRotation = 8f;
        [SerializeField]  float maxRandomRotation = 12f;

        public float RandomDirectionOffset => randomDirectionOffset;
        public float RandomAngleOffset => randomAngleOffset;
        public float MinRandomVelocity => minRandomVelocity;
        public float MaxRandomVelocity => maxRandomVelocity;
        public float MinRandomRotation => minRandomRotation;
        public float MaxRandomRotation => maxRandomRotation;
    }
}