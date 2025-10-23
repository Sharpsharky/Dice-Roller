namespace DiceRollingGame.Score
{
    using Dice;
    using Roller;
    using UnityEngine;
    using Zenject;

    public class ScoreManager : MonoBehaviour, IInitializable
    {
        private IRoller roller;
        private InterfaceUIView interfaceUIView;
        private int overallScore;

        [Inject]
        public void Construct(IRoller roller, InterfaceUIView interfaceUIView)
        {
            this.roller = roller;
            this.interfaceUIView = interfaceUIView;
        }

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