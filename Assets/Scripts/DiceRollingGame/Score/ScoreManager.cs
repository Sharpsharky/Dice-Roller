namespace DiceRollingGame.Score
{
    using Input;
    using Managers;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UnityEngine;

    public class ScoreManager : SerializedMonoBehaviour, IManager
    {
        [OdinSerialize] private IRoller roller;
        [SerializeField] private InterfaceUIView interfaceUIView;

        private int overallScore;
        
        public void Initialize()
        {
            roller.OnDiceTossed += WaitForDiceToFinishMoving;
            interfaceUIView.SetTossScoreAsUnknown();
            interfaceUIView.SetupOverallScore(0);
        }

        private void OnDestroy()
        {
            roller.OnDiceTossed -= WaitForDiceToFinishMoving;
        }

        private void WaitForDiceToFinishMoving(IDice dices)
        {
            interfaceUIView.SetTossScoreAsUnknown();
            dices.StartWaitingForRest();
            dices.OnFinishedMoving += AddScore;
        }

        private void AddScore(IDice dices, int score)
        {
            dices.OnFinishedMoving -= AddScore;
            
            overallScore += score;
            
            interfaceUIView.SetupTossScore(score);
            interfaceUIView.SetupOverallScore(overallScore);
        }
    }
}