using System;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    public class GameStats
    {
        [SerializeField] private int _playerScore = 0;
        public int PlayerScore { get => _playerScore; }

        public float TimeRemains
        {
            get
            {
                if (_dateTimeGameStarted == null)
                    return 0;

                float secondsDiff = (float)(DateTime.Now - _dateTimeGameStarted).TotalSeconds;
                return _gameSecondsDuration - secondsDiff;
            }
        }

        [SerializeField] private float _gameSecondsDuration = 120;

        [SerializeField] private DateTime _dateTimeGameStarted;
        [SerializeField] private bool _gameIsStarted = false;
        public bool GameIsStarted { get => _gameIsStarted; }

        public event Action<int> scoreUpdate;
        public event Action<bool> gameIsStarted;

        public void AddScore(int score)
        {
            _playerScore += score;

            scoreUpdate?.Invoke(_playerScore);
        }

        public void StartGame()
        {
            if (_gameIsStarted)
                return;

            _playerScore = 0;
            _gameIsStarted = true;
            _dateTimeGameStarted = DateTime.Now;
            gameIsStarted?.Invoke(true);
        }

        public void EndGame()
        {
            if (!_gameIsStarted)
                return;

            _gameIsStarted = false;
            gameIsStarted?.Invoke(false);
        }
    }
}

