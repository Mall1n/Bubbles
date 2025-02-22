using System;


namespace Game
{
    public static class GameSettings
    {
        private static float _playerScore = 0;
        public static float PlayerScore { get => _playerScore; }
        public static float TimeRemains
        {
            get
            {
                if (_dateTimeGameStarted == null)
                    return 0;

                float secondsDiff = (float)(DateTime.Now - _dateTimeGameStarted).TotalSeconds;
                return _gameSecondsDuration - secondsDiff;
            }
        }


        private static float _gameSecondsDuration = 120;
        // public static void SetGameTime(float time)
        // {
        //     if (time > 0)
        //         _gameSecondsDuration = time;
        // }
        public static float GameSecondsDuration { get => _gameSecondsDuration; }

        private static DateTime _dateTimeGameStarted;
        private static bool _gameIsStarted = false;
        public static bool GameIsStarted { get => _gameIsStarted; }

        public static event Action OnScoreUpdate;
        public static event Action<bool> GameChangedStatus;

        public static void AddScore(float score)
        {
            _playerScore += score;

            OnScoreUpdate?.Invoke();
        }

        public static void StartGame(float time)
        {
            if (_gameIsStarted)
                return;

            if (time > 0)
                _gameSecondsDuration = time;

            _playerScore = 0;
            _gameIsStarted = true;
            _dateTimeGameStarted = DateTime.Now;
            GameChangedStatus?.Invoke(true);
        }

        public static void EndGame()
        {
            if (!_gameIsStarted)
                return;

            _gameIsStarted = false;
            GameChangedStatus?.Invoke(false);
        }
    }
}

