// using System;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Bubbles
{
    // [System.Serializable]
    [CreateAssetMenu(fileName = "Bubbles Pool Config", menuName = "Bubbles Pool Config")]
    public class BubblesPoolConfig : ScriptableObject
    {
        [SerializeField] private BubbleHit[] _bubblesDefault;
        public IReadOnlyCollection<BubbleHit> BubblesDefault => _bubblesDefault;

        [SerializeField] private BubbleParticle[] _bubblesParticle;
        public IReadOnlyCollection<BubbleParticle> BubblesParticle => _bubblesParticle;

        [SerializeField] private BubbleHit[] _bubblesFast;
        public IReadOnlyCollection<BubbleHit> BubblesFast => _bubblesFast;

        [SerializeField] private BubbleParticle[] _bubblesParticleFast;
        public IReadOnlyCollection<BubbleParticle> BubblesParticleFast => _bubblesParticleFast;
    }
}
