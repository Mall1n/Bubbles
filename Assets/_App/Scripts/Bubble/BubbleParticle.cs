using UnityEngine;

namespace Bubbles
{
    public class BubbleParticle : Bubble<BubbleParticle>
    {
        protected new readonly float _explodeDelayTime = 3.0f;

        public override void LaunchBubble(float speedModificator = 1f)
        {
            Vector3 randomVectorPos = UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(0.01f, 1.0f);
            Vector3 posOrigin = this.transform.position;
            this.transform.position = randomVectorPos + posOrigin;

            this.transform.LookAt(posOrigin);

            float newScale = transform.localScale.x + UnityEngine.Random.Range(-0.05f, 0.1f);
            this.transform.localScale = new Vector3(newScale, newScale, newScale);

            _rb.AddRelativeForce(UnityEngine.Random.Range(_speedMin, _speedMax) * -this.transform.forward * speedModificator, ForceMode.Impulse);

            StartCoroutine(ExplodeWithTime(_explodeDelayTime + UnityEngine.Random.Range(-2.9f, 0.0f)));
        }
    }
}
