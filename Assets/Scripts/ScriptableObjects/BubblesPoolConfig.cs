// using System;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    // [System.Serializable]
    [CreateAssetMenu(fileName = "Bubbles Pool Config", menuName = "Bubbles Pool Config")]
    public class BubblesPoolConfig : ScriptableObject
    {
        [SerializeField] private BubbleHit[] _bubblesDefault;
        public IReadOnlyCollection<BubbleHit> BubblesDefault => _bubblesDefault;


        [SerializeField] private BubbleHit[] _bubblesFast;
        public IReadOnlyCollection<BubbleHit> BubblesFast => _bubblesFast;
    }
}
