
using System.Collections;
using UnityEngine;

namespace Bubbles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bubble : Poolable
    {
        [Header("Speed")]
        [SerializeField] protected float _speedMin = 1.5f;
        [SerializeField] protected float _speedMax = 3.0f;

        protected readonly float _explodeDelayTime = 20.0f;

        protected Rigidbody _rb;

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

        public override void Enable()
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        public override void Disable()
        {
            if (this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);
                
                InvokeDisabled(this);
            }
        }

        public override void OnCreated() { }
    }
}