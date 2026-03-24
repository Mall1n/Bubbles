
using System;
using System.Collections;
using UnityEngine;

namespace Bubbles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bubble<T> : MonoBehaviour, IPoolable<T> where T : Component
    {
        [Header("Speed")]
        [SerializeField] protected float _speedMin = 1.5f;
        [SerializeField] protected float _speedMax = 3.0f;

        protected readonly float _explodeDelayTime = 20.0f;

        protected Rigidbody _rb;

        public event Action<T> disabled;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public virtual void LaunchBubble(float speedModificator = 1f) { }

        protected virtual IEnumerator ExplodeWithTime(float time)
        {
            yield return new WaitForSeconds(time);

            Explode();
        }

        public virtual void Explode()
        {
            Disable();
        }

        public virtual void Enable()
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            if (this is T instance)
            {
                disabled?.Invoke(instance);
            }

            this.gameObject.SetActive(false);
        }

        public virtual void OnCreated() { }
    }
}