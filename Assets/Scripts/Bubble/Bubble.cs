
using System;
using System.Collections;
using UnityEngine;


namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bubble : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] protected float _speedMin = 1.5f;
        [SerializeField] protected float _speedMax = 3.0f;


        protected readonly float _destroyDelayTime = 30.0f;


        protected Rigidbody _rb;


        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void DestroyBubble()
        {
            Destroy(this.gameObject);
        }

        public virtual void LaunchBubble() { }

        // public virtual void LaunchBubble(Vector3 posOrigin) { }

        protected IEnumerator DestroyWithTime(float time)
        {
            yield return new WaitForSeconds(time);

            DestroyBubble();
        }
    }
}