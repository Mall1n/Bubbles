using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    public class ObjectPoolSettings
    {
        public string name;
        public int initialCount;
        public Transform container;
    }

    public class ObjectPool<T> : IDisposable where T : Poolable
    {
        private readonly string name;
        private readonly T[] _prefabs;
        private readonly Queue<T> _inactivePool;
        private readonly List<T> _activeObjects;
        private readonly Transform _container;

        private readonly HashSet<T> _prefabSet;
        private readonly HashSet<T> _activeSet;

        public int InactiveCount => _inactivePool.Count;
        public int ActiveCount => _activeObjects.Count;
        public int TotalCount => InactiveCount + ActiveCount;

        public event Action<T> objDisabled;

        public ObjectPool(T prefab, ObjectPoolSettings settings)
            : this(new T[] { prefab }, settings) { }

        public ObjectPool(T[] prefabs, ObjectPoolSettings settings)
        {
            name = settings.name;

            if (prefabs == null || prefabs.Length == 0)
            {
                Debug.LogError($"[ObjectPool<{typeof(T).Name}>.{settings.name}] Prefabs list is empty!");
                return;
            }

            _prefabs = prefabs;
            _prefabSet = new HashSet<T>(prefabs);
            _activeSet = new();
            _container = settings.container;
            _inactivePool = new Queue<T>(settings.initialCount);
            _activeObjects = new List<T>(settings.initialCount);

            for (int i = 0; i < settings.initialCount; i++)
            {
                // Choosing a prefab in a circle: 0, 1, 2, 0, 1, 2...
                T targetPrefab = _prefabs[i % _prefabs.Length];
                CreateNewObject(targetPrefab);
            }
        }

        public void Dispose()
        {
            foreach (var item in _inactivePool)
            {
                item.disabled -= OnObjDisabled;
            }
            foreach (var item in _activeObjects)
            {
                item.disabled -= OnObjDisabled;
            }
        }

        public bool Contains(T prefab)
        {
            return _prefabSet.Contains(prefab);
        }

        private void CreateNewObject(T prefab)
        {
            T obj = UnityEngine.Object.Instantiate(prefab, _container);
            obj.gameObject.SetActive(false);

            obj.disabled += OnObjDisabled;

            _inactivePool.Enqueue(obj);

            obj.OnCreated();
        }

        private void OnObjDisabled(Poolable poolable)
        {
            if (poolable is T p)
                if (_activeSet.Remove(p))
                {
                    _activeObjects.Remove(p);
                    _inactivePool.Enqueue(p);

                    objDisabled?.Invoke(p);
                }
        }

        public T Get()
        {
            if (_inactivePool.Count == 0)
            {
                ExpandPool();
            }

            if (_inactivePool.Count > 0)
            {
                T obj = _inactivePool.Dequeue();

                _activeObjects.Add(obj);
                _activeSet.Add(obj);

                obj.Enable();

                return obj;
            }

            Debug.LogWarning($"[ObjectPool<{typeof(T).Name}>.{name}] Critical! Cannot get an object from the pool!");
            return null;
        }

        private void ExpandPool()
        {
            Debug.Log($"[ObjectPool<{typeof(T).Name}>.{name}] Expanding pool");
            T targetPrefab = _prefabs[TotalCount % _prefabs.Length];
            CreateNewObject(targetPrefab);
        }

        public void DisableAll()
        {
            for (int i = _activeObjects.Count - 1; i >= 0; i--)
            {
                _activeObjects[i].Disable();
            }
        }
    }
}
