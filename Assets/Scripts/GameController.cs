using UnityEngine;


namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BubbleController _bubbleController;
        [SerializeField] private float _gameTimeDuration = 120;
        [SerializeField] private GameMenuInfo _gameMenuInfo;
        [SerializeField] private BubbleEventsAnimator _bubbleEventsAnimator;

        private bool _gameIsStarted => GameSettings.GameIsStarted;



        private void Update()
        {
            if (_gameIsStarted)
            {
                if (GameSettings.TimeRemains <= 0)
                {
                    StopGame();
                }
            }
        }

        private void Awake()
        {
            ShowGameMenuInfo(false);
        }

        private void OnEnable()
        {
            if (_gameMenuInfo != null)
                _gameMenuInfo.OnButtonStartNewGameClicked += StartGame;

            if (_bubbleController != null)
                _bubbleController.OnBubbleDestroy += BubbleDestroyShowEvent;
        }

        private void OnDisable()
        {
            if (_gameMenuInfo != null)
                _gameMenuInfo.OnButtonStartNewGameClicked -= StartGame;

            if (_bubbleController != null)
                _bubbleController.OnBubbleDestroy -= BubbleDestroyShowEvent;
        }

        private void BubbleDestroyShowEvent(BubbleEventsAnimator.Type type)
        {
            if (_bubbleEventsAnimator != null)
                _bubbleEventsAnimator.OnBubbleDestroy(type);
        }

        private void ShowGameMenuInfo(bool showScore)
        {
            if (_gameMenuInfo != null)
            {
                if (showScore)
                    _gameMenuInfo.ShowGameMenuInfo((int)GameSettings.PlayerScore);
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

            GameSettings.StartGame(_gameTimeDuration);

            HideGameMenuInfo();

            if (_bubbleController != null)
                _bubbleController.EnableController();
        }

        [ContextMenu("Stop Game")]
        private void StopGame()
        {
            if (!_gameIsStarted)
                return;

            Debug.Log("Game is ended");

            // _gameIsStarted = false;
            GameSettings.EndGame();

            ShowGameMenuInfo(true);

            if (_bubbleController != null)
                _bubbleController.DisableController();
        }
    }
}