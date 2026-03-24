using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    public class ObjectPoolSettings
    {
        public int initialCount;
        public Transform container;
    }

    public class ObjectPool<T> : IDisposable where T : Component, IPoolable<T>
    {
        private readonly T[] _prefabs;
        private readonly Queue<T> _inactivePool;
        private readonly List<T> _activeObjects;
        private readonly Transform _container;

        public int InactiveCount => _inactivePool.Count;
        public int ActiveCount => _activeObjects.Count;
        public int TotalCount => InactiveCount + ActiveCount;

        public event Action<T> objDisabled;

        public ObjectPool(T prefab, ObjectPoolSettings settings)
            : this(new T[] { prefab }, settings) { }

        public ObjectPool(T[] prefabs, ObjectPoolSettings settings)
        {
            if (prefabs == null || prefabs.Length == 0)
            {
                Debug.LogError($"[ObjectPool<{typeof(T).Name}>] Список префабов пуст!");
                return;
            }

            _prefabs = prefabs;
            _container = settings.container;
            _inactivePool = new Queue<T>(settings.initialCount);
            _activeObjects = new List<T>(settings.initialCount);

            for (int i = 0; i < settings.initialCount; i++)
            {
                // Выбираем префаб по кругу: 0, 1, 2, 0, 1, 2...
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
            return _prefabs.Contains(prefab);
        }

        private void CreateNewObject(T prefab)
        {
            T obj = UnityEngine.Object.Instantiate(prefab, _container);
            obj.gameObject.SetActive(false);

            obj.disabled += OnObjDisabled;

            _inactivePool.Enqueue(obj);

            obj.OnCreated();
        }

        private void OnObjDisabled(T poolable)
        {
            if (_activeObjects.Contains(poolable))
            {
                _activeObjects.Remove(poolable);
                _inactivePool.Enqueue(poolable);
                objDisabled?.Invoke(poolable);
            }
        }

        public T Get()
        {
            if (_inactivePool.Count > 0)
            {
                T obj = _inactivePool.Dequeue();
                obj.Enable();
                _activeObjects.Add(obj);
                return obj;
            }

            Debug.Log($"[ObjectPool<{typeof(T).Name}>] Создан новый объект (достигнут лимит пула)");
            T targetPrefab = _prefabs[TotalCount % _prefabs.Length];
            CreateNewObject(targetPrefab);

            if (_inactivePool.Count > 0)
            {
                T obj = _inactivePool.Dequeue();
                obj.Enable();
                _activeObjects.Add(obj);
                return obj;
            }

            Debug.LogWarning($"[ObjectPool<{typeof(T).Name}>] Не удалось расширить пул!");
            return null;
        }

        public void DisableAll()
        {
            foreach (T obj in _activeObjects.ToList())
            {
                obj.Disable();
            }
        }
    }
}
