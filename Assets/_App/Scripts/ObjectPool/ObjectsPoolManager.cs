using System;
using System.Linq;
using UnityEngine;

namespace Bubbles
{
    public class ObjectsPoolManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BubblesPoolConfig _bubblesPoolConfig;

        [Header("Pool Bubbles")]
        [SerializeField] private ObjectPoolInstance<BubbleHit> _poolBubbles;
        [SerializeField] private ObjectPoolInstance<BubbleParticle> _poolParticles;

        [SerializeField] private ObjectPoolInstance<BubbleHit> _poolBubblesFast;
        [SerializeField] private ObjectPoolInstance<BubbleParticle> _poolParticlesFast;

        public static ObjectsPoolManager Instance { get; private set; }

        public ObjectPoolInstance<BubbleHit> PoolBubbles { get => _poolBubbles; }
        public ObjectPoolInstance<BubbleParticle> PoolParticles { get => _poolParticles; }
        public ObjectPoolInstance<BubbleHit> PoolBubblesFast { get => _poolBubblesFast; }
        public ObjectPoolInstance<BubbleParticle> PoolParticlesFast { get => _poolParticlesFast; }

        public event Action<BubbleHit> bubbleDisabled;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            _poolBubbles.Init(_bubblesPoolConfig.BubblesDefault.ToArray());
            _poolParticles.Init(_bubblesPoolConfig.BubblesParticle.ToArray());

            _poolBubblesFast.Init(_bubblesPoolConfig.BubblesFast.ToArray());
            _poolParticlesFast.Init(_bubblesPoolConfig.BubblesParticleFast.ToArray());

            _poolBubbles.objDisabled += OnBubbleDisabled;
            _poolBubblesFast.objDisabled += OnBubbleDisabled;
        }

        private void OnBubbleDisabled(BubbleHit bubble) => bubbleDisabled?.Invoke(bubble);

        public IPoolInstance ContainsPrefab<T>(T prefab) where T : Component, IPoolable<T> 
        {
            if (_poolBubbles.ContainsPrefab(prefab as BubbleHit)) return _poolBubbles;
            if (_poolParticles.ContainsPrefab(prefab as BubbleParticle)) return _poolParticles;
            if (_poolBubblesFast.ContainsPrefab(prefab as BubbleHit)) return _poolBubblesFast;
            if (_poolParticlesFast.ContainsPrefab(prefab as BubbleParticle)) return _poolParticlesFast;

            return null;
        }

        private void OnDestroy()
        {
            _poolBubbles.objDisabled -= OnBubbleDisabled;
            _poolBubblesFast.objDisabled -= OnBubbleDisabled;

            _poolBubbles?.Dispose();
            _poolParticles?.Dispose();

            _poolBubblesFast?.Dispose();
            _poolParticlesFast?.Dispose();
        }
    }
}
