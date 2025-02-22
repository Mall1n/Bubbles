using System;
using UnityEngine;


namespace Game
{
    public class BubbleHit : Bubble
    {
        [Header("Score")]
        [SerializeField] protected float _score = 10;
        [SerializeField] protected MiniBubble _bubbleMini;


        public event Action<float> OnBubbleDestroed;

        protected virtual void OnDisable()
        {
            // Debug.Log("OnDisable");

            StopAllCoroutines();
        }

        protected virtual void DestroyBubbleByPlayer()
        {
            SpawnMiniBubbles(20);

            OnBubbleDestroed?.Invoke(_score);

            Destroy(this.gameObject);
        }

        public virtual void OnRayHit()
        {
            // Debug.Log($"OnRayHit");

            DestroyBubbleByPlayer();
        }

        // protected override void DestroyBubble()
        // {
        //     Destroy(this.gameObject);
        // }

        protected virtual void SpawnMiniBubbles(int amountMiniBubbles)
        {
            if (_bubbleMini == null)
                return;

            int amountMiniBubblesTotal = amountMiniBubbles + UnityEngine.Random.Range(-5, 5);

            for (int i = 0; i < amountMiniBubblesTotal; i++)
            {
                MiniBubble miniBubble = Instantiate(_bubbleMini, this.transform.position, Quaternion.identity);
                miniBubble.LaunchBubble();
            }
        }

        public override void LaunchBubble()
        {
            float speed = UnityEngine.Random.Range(_speedMin, _speedMax);
            _rb.AddRelativeForce(speed * transform.forward, ForceMode.Impulse);
            _score += speed * 10;

            StartCoroutine(DestroyWithTime(_destroyDelayTime));
        }
    }
}
