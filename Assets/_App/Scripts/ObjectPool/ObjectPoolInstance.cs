using System;
using UnityEngine;

namespace Bubbles
{
    public interface IPoolInstance
    {
        bool ContainsPrefab(IPoolable prefab);

        Component GetComponent();
    }

    [Serializable]
    public class ObjectPoolInstance<T> : IPoolInstance, IDisposable where T : Component, IPoolable<T>
    {
        [SerializeField] private ObjectPoolSettings _poolSettings;
        [SerializeField] private ObjectPool<T> _poolObjets;

        public event Action<T> objDisabled;

        public void Init(T[] objs)
        {
            _poolObjets = new ObjectPool<T>(objs, _poolSettings);

            _poolObjets.objDisabled += HandleObjectDisabled;
        }

        public void Dispose()
        {
            _poolObjets.objDisabled -= HandleObjectDisabled;

            _poolObjets?.Dispose();
        }

        private void HandleObjectDisabled(T obj) => objDisabled?.Invoke(obj);

        public bool ContainsPrefab(IPoolable prefab)
        {
            if (prefab is T poolable)
                return _poolObjets.Contains(poolable);
            else return false;
        }

        public T Get()
        {
            return _poolObjets.Get();
        }

        public Component GetComponent()
        {
            return this.Get();
        }

        public void DisableAll()
        {
            _poolObjets.DisableAll();
        }
    }
}
