using System;
using System.Collections;
using UnityEngine;

namespace Bubbles
{
    [RequireComponent(typeof(SphereCollider))]
    public class BubbleHit : Bubble<BubbleHit>, ITouchable
    {
        [Header("Score")]
        [SerializeField] protected int _score = 10;
        [SerializeField] protected Type _type = Type.Default;
        [SerializeField] protected BubbleParticle particleBubble;
        [SerializeField] protected bool _isHittable = true;

        private SphereCollider _collider;

        public int Score { get => _score; }
        public Type BubbleType { get => _type; }
        public bool IsHittable { get => _isHittable; set => _isHittable = value; }
        public BubbleParticle ParticleBubble { get => particleBubble; }
        public SphereCollider Collider { get => _collider; }

        public static event Action<BubbleHit> spawnParticles;
        public static event Action<BubbleHit> playerTouch;

        public enum Type
        {
            Default = 0,
            Fast = 1
        }

        protected override void Awake()
        {
            base.Awake();

            _collider = GetComponent<SphereCollider>();
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void OnCreated()
        {
            _collider.enabled = false;
        }

        public void ExplodeWithoutParticles()
        {
            base.Explode();
        }

        public override void LaunchBubble(float speedModificator = 1f)
        {
            float speed = UnityEngine.Random.Range(_speedMin, _speedMax);
            speed *= speedModificator;
            _rb.AddRelativeForce(speed * transform.forward, ForceMode.Impulse);
            _score += (int)(speed * 10f);

            StartCoroutine(ExplodeWithTime(_explodeDelayTime));
        }

        protected override IEnumerator ExplodeWithTime(float time)
        {
            yield return new WaitForSeconds(time);

            ExplodeWithoutParticles();
        }

        public override void Explode()
        {
            base.Explode();

            spawnParticles?.Invoke(this);
        }

        public override void Disable()
        {
            base.Disable();

            _collider.enabled = false;
        }

        public bool OnTouch()
        {
            if (!_isHittable)
                return false;

            Explode();

            playerTouch?.Invoke(this);

            return true;
        }
    }
}
