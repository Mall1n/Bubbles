using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    public class BubbleController : MonoBehaviour
    {
        [Header("Spawn Bubble Settings")]
        [SerializeField] private float _minRadius = 5f;
        [SerializeField] private float _maxRadius = 8f;
        [SerializeField] private Vector2 _timeSpawnBubble = new(2.5f, 4.5f);
        [SerializeField] private ObjectsPoolManager objectsPoolManager;

        [SerializeField][ReadOnly] private float _complicateIntensitySpawnBubbles;
        [SerializeField][ReadOnly] private float _chanceSpawnSpecialBubble = 10.0f;
        [SerializeField][ReadOnly] private float _bubblesSpeedModificatorGlobal = 1f;

        [Header("--- Only for View ---")]
        [SerializeField] private List<BubbleHit> _gameBubbles = new();

        public event Action<BubbleHit.Type, int> bubbleOnPlayerTouched; // int score

        private void Awake()
        {
            if (_minRadius >= _maxRadius)
            {
                Debug.Log("minRadius grather then maxRadius!");
            }
        }

        private void OnEnable()
        {
            BubbleHit.spawnParticles += OnBubbleSpawnParticle;
            BubbleHit.playerTouch += OnBubblePlayerTouch;

            objectsPoolManager.bubbleDisabled += OnBubbleHitDisabled;

            StopGame();
        }

        private void OnDisable()
        {
            BubbleHit.spawnParticles -= OnBubbleSpawnParticle;
            BubbleHit.playerTouch -= OnBubblePlayerTouch;

            objectsPoolManager.bubbleDisabled -= OnBubbleHitDisabled;

            StopAllCoroutines();
        }

        private void OnBubblePlayerTouch(BubbleHit bubble)
        {
            bubbleOnPlayerTouched?.Invoke(bubble.BubbleType, bubble.Score);
        }

        private void OnBubbleHitDisabled(BubbleHit bubble)
        {
            _gameBubbles.Remove(bubble);
        }

        private void SpawnRandomBubble(bool touchable)
        {
            if (UnityEngine.Random.Range(0.0f, 100.0f) <= _chanceSpawnSpecialBubble)
            {
                SpawnBubbleSpecial(touchable);
            }
            else
            {
                SpawnBubbleDefault(touchable);
            }
        }

        public void StartGame()
        {
            _chanceSpawnSpecialBubble = 10f;
            _complicateIntensitySpawnBubbles = 0f;
            _bubblesSpeedModificatorGlobal = 1f;

            StopAllCoroutines();
            DestroyAllBubbles();

            StartCoroutine(SpawnBubbles(true));
            StartCoroutine(ComplicateIntensitySpawnBubbles());
            StartCoroutine(ComplicateChanceSpawnSpecialBubble());
        }

        public void StopGame()
        {
            _complicateIntensitySpawnBubbles = -2f;
            _bubblesSpeedModificatorGlobal = 0.6f;

            StopAllCoroutines();

            DisableHittableAllBubbles();

            StartCoroutine(SpawnBubbles(false));
        }

        private IEnumerator SpawnBubbles(bool touchable)
        {
            while (true)
            {
                float timeDelay = UnityEngine.Random.Range(_timeSpawnBubble.x - _complicateIntensitySpawnBubbles, _timeSpawnBubble.y - _complicateIntensitySpawnBubbles);
                yield return new WaitForSeconds(timeDelay);

                SpawnRandomBubble(touchable);

                Physics.SyncTransforms();
            }
        }

        private const float _complicateIntensitySpawnBubblesMax = 1.5f;

        private IEnumerator ComplicateIntensitySpawnBubbles()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                _complicateIntensitySpawnBubbles += 0.01f;
                if (_complicateIntensitySpawnBubbles > _complicateIntensitySpawnBubblesMax)
                {
                    _complicateIntensitySpawnBubbles = _complicateIntensitySpawnBubblesMax;
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
                if (_chanceSpawnSpecialBubble > 50f)
                {
                    _chanceSpawnSpecialBubble = 50f;
                    yield break;
                }
            }
        }

        private void SpawnBubbleDefault(bool touchable)
        {
            BubbleHit bubblePrefab = objectsPoolManager.PoolBubbles.Get();
            SpawnBubble(bubblePrefab, touchable);
        }

        private void SpawnBubbleSpecial(bool touchable)
        {
            BubbleHit bubblePrefab = objectsPoolManager.PoolBubblesFast.Get();
            SpawnBubble(bubblePrefab, touchable);
        }

        private void SpawnBubble(BubbleHit bubblePrefab, bool touchable)
        {
            Vector2 randomPosition = GetRandomPositionInRing();

            bubblePrefab.IsHittable = touchable;

            Transform bubbleTransform = bubblePrefab.transform;

            bubbleTransform.position = randomPosition; // z -> 0
            bubbleTransform.LookAt(Vector3.zero);
            bubbleTransform.Translate(new Vector3(0, 0, FRange(-1f, 1.5f)), Space.World);
            bubbleTransform.Rotate(FRange(-25.0f, 25.0f), 0, 0);

            bubblePrefab.LaunchBubble(_bubblesSpeedModificatorGlobal);

            _gameBubbles.Add(bubblePrefab);
        }

        private static float FRange(float min, float max) => UnityEngine.Random.Range(min, max);

        private void OnBubbleSpawnParticle(BubbleHit bubble)
        {
            SpawnParticleBubbles(bubble);
        }

        protected virtual void SpawnParticleBubbles(BubbleHit bubble)
        {
            BubbleParticle bubbleParticle = bubble.ParticleBubble;
            if (bubbleParticle == null)
                return;

            IPoolInstance poolInstance = objectsPoolManager.ContainsPrefab<BubbleParticle>(bubbleParticle);
            BubbleParticle particle;

            if (poolInstance != null)
            {
                int amountBubbleParticles = UnityEngine.Random.Range(15, 30);

                for (int i = 0; i < amountBubbleParticles; i++)
                {
                    particle = poolInstance.GetComponent() as BubbleParticle;
                    particle.transform.position = bubble.transform.position;
                    particle.gameObject.SetActive(true);
                    particle.LaunchBubble();
                }
            }
        }

        private void DisableHittableAllBubbles()
        {
            foreach (var bubble in _gameBubbles)
            {
                if (bubble != null)
                    bubble.IsHittable = false;
            }
        }

        private void DestroyAllBubbles()
        {
            var bubblesCopy = new List<BubbleHit>(_gameBubbles);

            foreach (var bubble in bubblesCopy)
            {
                if (bubble != null)
                    bubble.Explode();
            }

            _gameBubbles.Clear();
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
