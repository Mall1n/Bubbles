using System;
using UnityEngine;

namespace Bubbles
{
    public interface IPoolable<T> where T : Component
    {
        public event Action<T> disabled;

        void OnCreated();

        void Enable();
        void Disable();
    }
}
