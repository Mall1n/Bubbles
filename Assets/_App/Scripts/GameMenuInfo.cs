using System;
using TMPro;
using UnityEngine;


namespace Bubbles
{
    public class GameMenuInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _score;

        private string _scoreTextOrigin;
        public event Action OnButtonStartNewGameClicked;

        private void Start()
        {
            _scoreTextOrigin = _score.text;
        }

        public void ButtonStartNewGameClicked() => OnButtonStartNewGameClicked?.Invoke();

        public void ShowGameMenuInfo()
        {
            this.gameObject.SetActive(true);

            SetActiveScore(false);
        }
        public void ShowGameMenuInfo(int score)
        {
            this.gameObject.SetActive(true);

            SetActiveScore(true);
            SetPlayerScore(score);
        }

        public void HideGameMenuInfo()
        {
            this.gameObject.SetActive(false);
        }

        private void SetActiveScore(bool value)
        {
            if (_score != null)
                _score.gameObject.SetActive(value);
        }

        private void SetPlayerScore(int score)
        {
            _score.text = _scoreTextOrigin + score;
        }
    }
}
