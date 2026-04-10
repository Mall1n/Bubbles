using System;
using UnityEngine;

namespace Bubbles
{
    public interface IPoolInstance
    {
        bool ContainsPrefab(Poolable prefab);

        Component GetComponent();
    }

    [Serializable]
    public class ObjectPoolInstance<T> : IPoolInstance, IDisposable where T : Poolable
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

        public bool ContainsPrefab(Poolable prefab)
        {
            return _poolObjets.Contains(prefab as T);
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
