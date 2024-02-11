namespace DiceRollingGame.Score
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class InterfaceUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text thisTossScoreText;
        [SerializeField] private TMP_Text overallScoreText;
        [SerializeField] private Button RollButton;

        [SerializeField] private string thisTossPrefix = "This toss:";
        [SerializeField] private string overallScorePrefix = "Overall:";

        public void SetTossScoreAsUnknown()
        {
            thisTossScoreText.text = $"{thisTossPrefix} ?";
        }
        
        public void SetupTossScore(int tossScore)
        {
            thisTossScoreText.text = $"{thisTossPrefix} {tossScore.ToString()}";
        }

        public void SetupOverallScore(int overallScore)
        {
            overallScoreText.text = $"{overallScorePrefix} {overallScore.ToString()}";
        } 
    }
}