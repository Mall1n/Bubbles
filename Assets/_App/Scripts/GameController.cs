using UnityEngine;

namespace Bubbles
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BubbleController _bubbleController;
        [SerializeField] private GameMenuInfo _gameMenuInfo;
        [SerializeField] private BubbleEventsAnimator _bubbleEventsAnimator;
        [SerializeField] private GameStats gameStats;
        [SerializeField] private PlayerScoreUIUpdate playerScoreUIUpdate;

        private bool _gameIsStarted => gameStats.GameIsStarted;

        private void Update()
        {
            if (_gameIsStarted)
            {
                playerScoreUIUpdate.UpdateTimeRemains(gameStats.TimeRemains);

                if (gameStats.TimeRemains <= 0)
                {
                    StopGame();
                }
            }
        }

        private void Awake()
        {
            ShowGameMenuInfo(false);

#if UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }

        private void OnEnable()
        {
            gameStats.gameIsStarted += OnGameIsStarted;
            gameStats.scoreUpdate += OnScoreUpdate;

            _gameMenuInfo.OnButtonStartNewGameClicked += StartGame;

            _bubbleController.bubbleOnPlayerTouched += BubbleDestroyShowEvent;
        }

        private void OnDisable()
        {
            gameStats.gameIsStarted -= OnGameIsStarted;
            gameStats.scoreUpdate -= OnScoreUpdate;

            _gameMenuInfo.OnButtonStartNewGameClicked -= StartGame;

            _bubbleController.bubbleOnPlayerTouched -= BubbleDestroyShowEvent;
        }

        private void OnScoreUpdate(int score)
        {
            playerScoreUIUpdate.UpdateScore(score);
        }

        private void OnGameIsStarted(bool isStarted)
        {
            if (isStarted)
            {
                playerScoreUIUpdate.ResetScore();
            }
            else
            {
                playerScoreUIUpdate.ResetTimer();
            }
        }

        private void BubbleDestroyShowEvent(BubbleHit.Type type, int score)
        {
            if (type == BubbleHit.Type.Fast)
                _bubbleEventsAnimator.OnBubbleDestroy(BubbleEventsAnimator.Type.Perfect);

            gameStats.AddScore(score);
        }

        private void ShowGameMenuInfo(bool showScore)
        {
            if (_gameMenuInfo != null)
            {
                if (showScore)
                    _gameMenuInfo.ShowGameMenuInfo(gameStats.PlayerScore);
                else
                    _gameMenuInfo.ShowGameMenuInfo();
            }
        }

        private void HideGameMenuInfo()
        {
            if (_gameMenuInfo != null)
            {
                _gameMenuInfo.HideGameMenuInfo();
            }
        }

        [ContextMenu("Start Game")]
        private void StartGame()
        {
            if (_gameIsStarted)
                return;

            Debug.Log("Game is started");

            gameStats.StartGame();

            HideGameMenuInfo();

            if (_bubbleController != null)
                _bubbleController.StartGame();
        }

        [ContextMenu("Stop Game")]
        private void StopGame()
        {
            if (!_gameIsStarted)
                return;

            Debug.Log("Game is ended");

            gameStats.EndGame();

            ShowGameMenuInfo(true);

            if (_bubbleController != null)
                _bubbleController.StopGame();
        }
    }
}