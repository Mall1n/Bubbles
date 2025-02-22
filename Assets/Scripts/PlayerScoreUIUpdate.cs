using TMPro;
using UnityEngine;


namespace Game
{
    public class PlayerScoreUIUpdate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerScore;
        [SerializeField] private TextMeshProUGUI _timeRemains;


        private bool _enabled = false;

        private void OnEnable()
        {
            GameSettings.OnScoreUpdate += UpdateScore;
            GameSettings.GameChangedStatus += SetUpdateUI;
        }

        private void OnDisable()
        {
            GameSettings.OnScoreUpdate -= UpdateScore;
            GameSettings.GameChangedStatus -= SetUpdateUI;
        }

        private void SetUpdateUI(bool value)
        {
            _enabled = value;
            if (value == true)
                _playerScore.text = "0";
            else
                _timeRemains.text = "-";
        }

        private void Update()
        {
            if (!_enabled)
                return;

            _timeRemains.text = ((int)GameSettings.TimeRemains).ToString();
        }

        private void UpdateScore()
        {
            _playerScore.text = ((int)GameSettings.PlayerScore).ToString();
        }
    }
}
