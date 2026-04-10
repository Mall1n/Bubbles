using System;
using UnityEngine;

namespace Bubbles
{
    public interface IPoolable
    {
        
    }
    
    public interface IPoolable<T> : IPoolable where T : Component
    {
        event Action<T> disabled;
        
        void OnCreated();
        void Enable();
        void Disable();
    }
}
