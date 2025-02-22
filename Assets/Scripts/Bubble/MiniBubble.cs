using UnityEngine;


namespace Game
{
    public class MiniBubble : Bubble
    {
        protected new readonly float _destroyDelayTime = 3.0f;

        public override void LaunchBubble()
        {
            Vector3 randomVectorPos = Random.insideUnitSphere.normalized * Random.Range(0.01f, 1.0f);
            Vector3 posOrigin = this.transform.position;
            this.transform.position = randomVectorPos + posOrigin;

            this.transform.LookAt(posOrigin);

            float newScale = transform.localScale.x + Random.Range(-0.05f, 0.1f);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);

            _rb.AddRelativeForce(Random.Range(_speedMin, _speedMax) * -this.transform.forward, ForceMode.Impulse);

            StartCoroutine(DestroyWithTime(_destroyDelayTime + Random.Range(-2.9f, 0.0f))); // add check on time
        }
    }
}
