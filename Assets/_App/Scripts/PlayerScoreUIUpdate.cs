using TMPro;
using UnityEngine;


namespace Bubbles
{
    public class PlayerScoreUIUpdate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerScore;
        [SerializeField] private TextMeshProUGUI _timeRemains;

        public void ResetScore()
        {
            _playerScore.text = "0";
        }

        public void ResetTimer()
        {
            _timeRemains.text = "-";
        }

        public void UpdateTimeRemains(float timeRemains)
        {
            _timeRemains.text = timeRemains.ToString("F1");
        }

        public void UpdateScore(int playerScore)
        {
            _playerScore.text = playerScore.ToString();
        }
    }
}
