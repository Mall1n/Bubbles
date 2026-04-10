using System;
using UnityEngine;

namespace Bubbles
{
    public interface IPoolable
    {
        // event Action<IPoolable> disabled;

        void OnCreated();
        void Enable();
        void Disable();
    }

    public abstract class Poolable : MonoBehaviour, IPoolable
    {
        public event Action<Poolable> disabled;

        public abstract void Disable();

        public abstract void Enable();

        public abstract void OnCreated();

        protected void InvokeDisabled(Poolable poolable) => disabled?.Invoke(poolable);
    }
}
