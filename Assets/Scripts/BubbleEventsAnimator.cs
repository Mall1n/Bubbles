using UnityEngine;


namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class BubbleEventsAnimator : MonoBehaviour
    {
        private Animator _animator;

        public enum Type
        {
            None = 0,
            Perfect = 1
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnBubbleDestroy(Type type)
        {
            if (type == Type.Perfect)
                _animator.SetTrigger("OnPerfect");
        }
    }
}
