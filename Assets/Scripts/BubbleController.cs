using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace Game
{
    public class BubbleController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BubblesPoolConfig _bubblesPoolConfig;


        [Header("Spawn Bubble Settings")]
        [SerializeField] private float _minRadius = 5f;
        [SerializeField] private float _maxRadius = 8f;
        [SerializeField][Range(100, 500)] private int _intensitySpawnBubbles = 300;
        [SerializeField][Range(0.5f, 10.0f)] private float _timeToGuaranteedSpawnBubble = 3.0f;




        private BubbleHit[] _defaultBubbles;
        private int _defaultBubblesCount;
        private BubbleHit[] _fastBubbles;
        private int _fastBubblesCount;

        private bool _enabled = false;
        private int _reverseIntensitySpawnBubbles;
        private float _chanceSpawnSpecialBubble = 10.0f;

        private readonly float _anglesBubbleShiftLaunch = 27.5f;

        public event Action<BubbleEventsAnimator.Type> OnBubbleDestroy;




        private void Awake()
        {
            if (_minRadius >= _maxRadius)
            {
                Log.Write("minRadius grather then maxRadius!", LogType.Warning);
                return;
            }

            _defaultBubbles = _bubblesPoolConfig.BubblesDefault.ToArray();
            _defaultBubblesCount = _defaultBubbles.Length;

            _fastBubbles = _bubblesPoolConfig.BubblesFast.ToArray();
            _fastBubblesCount = _fastBubbles.Length;
        }

        private void FixedUpdate() // 90 // move to coroutine 
        {
            if (!_enabled)
                return;

            if (UnityEngine.Random.Range(0, _reverseIntensitySpawnBubbles) == 1)
                SpawnRandomBubble();
        }

        private void SpawnRandomBubble()
        {
            if (UnityEngine.Random.Range(0.0f, 100.0f) <= _chanceSpawnSpecialBubble)
                SpawnBubbleSpecial();
            else
                SpawnBubbleDefault();
        }

        public void EnableController()
        {
            _enabled = true;

            StartCoroutine(SpawnGuaranteedBubble());
            StartCoroutine(ChangeSpawnComplicateBubbles());

            _reverseIntensitySpawnBubbles = 600 - _intensitySpawnBubbles + (int)GameSettings.GameSecondsDuration;

            if (_reverseIntensitySpawnBubbles < 100)
                _reverseIntensitySpawnBubbles = 100;
            else
                StartCoroutine(ComplicateChanceSpawnSpecialBubble());
        }

        public void DisableController()
        {
            _enabled = false;

            StopAllCoroutines();
        }

        private IEnumerator SpawnGuaranteedBubble()
        {
            while (true)
            {
                SpawnRandomBubble();

                yield return new WaitForSeconds(_timeToGuaranteedSpawnBubble);
                // yield return null;
            }
        }

        private IEnumerator ChangeSpawnComplicateBubbles()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                _reverseIntensitySpawnBubbles -= 2;
                if (_reverseIntensitySpawnBubbles < 100)
                {
                    _reverseIntensitySpawnBubbles = 100;
                    yield break;
                }
            }
        }

        private IEnumerator ComplicateChanceSpawnSpecialBubble()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                _chanceSpawnSpecialBubble += 0.1f;
                if (_chanceSpawnSpecialBubble > 50)
                {
                    _chanceSpawnSpecialBubble = 50;
                    yield break;
                }
            }
        }

        private void SpawnBubbleDefault()
        {
            int index = UnityEngine.Random.Range(0, _defaultBubblesCount);
            BubbleHit bubble = _defaultBubbles[index];
            if (bubble != null)
                SpawnBubble(bubble, BubbleEventsAnimator.Type.None);
        }

        private void SpawnBubbleSpecial() // add chances
        {
            int index = UnityEngine.Random.Range(0, _fastBubblesCount);
            BubbleHit bubble = _fastBubbles[index];

            if (bubble != null)
            {
                SpawnBubble(bubble, BubbleEventsAnimator.Type.Perfect);
            }
        }

        private void SpawnBubble(BubbleHit bubblePrefab, BubbleEventsAnimator.Type type)
        {
            Vector2 randomPosition = GetRandomPositionInRing();

            BubbleHit bubble = Instantiate(
                bubblePrefab,
                Vector3.zero,
                Quaternion.identity,
                this.transform
            );

            if (type == BubbleEventsAnimator.Type.None)
            {
                bubble.OnBubbleDestroed += (score) => GameSettings.AddScore(score);
            }
            else
            {
                bubble.OnBubbleDestroed += (score) =>
                {
                    GameSettings.AddScore(score);

                    OnBubbleDestroy?.Invoke(type);
                };
            }


            Transform bubbleTransform = bubble.transform;

            bubbleTransform.localPosition = new Vector3(randomPosition.x, randomPosition.y, UnityEngine.Random.Range(-1.5f, 1.5f));
            bubbleTransform.LookAt(this.transform);
            bubbleTransform.localPosition = new Vector3(bubbleTransform.localPosition.x, bubbleTransform.localPosition.y, UnityEngine.Random.Range(-1.5f, 1.5f) + bubbleTransform.localPosition.z);

            Vector3 bubbleEuler = bubbleTransform.localEulerAngles;
            float _algleShift = UnityEngine.Random.Range(-_anglesBubbleShiftLaunch, _anglesBubbleShiftLaunch);
            bubbleTransform.localEulerAngles = new Vector3(bubbleEuler.x + _algleShift, bubbleEuler.y, bubbleEuler.z);

            bubble.LaunchBubble();
        }

        private Vector2 GetRandomPositionInRing()
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

            float randomRadius = Mathf.Sqrt(UnityEngine.Random.Range(_minRadius * _minRadius, _maxRadius * _maxRadius));

            return randomDirection * randomRadius;
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.cyan;
        //     Gizmos.DrawWireSphere(transform.position, _minRadius);
        //     Gizmos.DrawWireSphere(transform.position, _maxRadius);
        // }
    }
}
