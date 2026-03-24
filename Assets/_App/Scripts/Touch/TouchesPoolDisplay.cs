using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    [Serializable]
    public class TouchId
    {
        public string id;
        public TouchPoint position;
        public Touch touch;
        public float lastUpdateTime;

        public void ResetValues()
        {
            id = string.Empty;
            position = new(0, 0);
            lastUpdateTime = 0;
        }
    }

    public class TouchesPoolDisplay : MonoBehaviour
    {
        [Header("Main settings")]
        [SerializeField] private LidarTouchesHandler _targetLidarTouches;
        [SerializeField] private Canvas _canvas;

        [Header("Pool Settings")]
        [SerializeField] private short _poolObjectsCount = 10;
        [SerializeField] private Touch _targetObjectPool;

#if UNITY_EDITOR
        [Header("EDITOR")]
        [SerializeField] private bool _log = false;
#endif

        private Vector2 _rectSize;

        private TouchId[] _touchesPool;

        private readonly Dictionary<string, TouchId> _activeTouches = new();

        private void Awake()
        {
            _rectSize = this.transform.parent.GetComponent<RectTransform>().rect.size;

            InitPool();
        }

        private void OnEnable()
        {
            _targetLidarTouches.dataTouchesUpdated += OnTouchesUpdated;
        }

        private void OnDisable()
        {
            _targetLidarTouches.dataTouchesUpdated -= OnTouchesUpdated;
        }

#if UNITY_EDITOR
        [ContextMenu("PrintActiveTouches")]
        private void PrintActiveTouches()
        {
            print(string.Join(", ", _activeTouches.Values));
        }
#endif

        private void InitPool()
        {
            _touchesPool = new TouchId[_poolObjectsCount];
            for (int i = 0; i < _poolObjectsCount; i++)
            {
                Touch touch = Instantiate<Touch>(_targetObjectPool, this.transform);
                touch.Init(_canvas);
                touch.Disable();

                _touchesPool[i] = new TouchId()
                {
                    touch = touch,
                    id = string.Empty,
                    lastUpdateTime = 0,
                    position = new(0, 0)
                };
            }
        }

        private void OnTouchesUpdated(TouchData[] touchesData)
        {
            float time = Time.time;

            foreach (TouchData touchData in touchesData)
            {
                string id = touchData.id;
                TouchData.State state = touchData.state;
                TouchPoint posNormalized = touchData.posNormalized;

                // --- ALREADY EXIST ---
                if (_activeTouches.TryGetValue(id, out TouchId touchId))
                {
                    if (state == TouchData.State.Remove)
                    {
                        RemoveTouchFromActive(touchId, id);
                    }
                    else
                    {
                        // --- UPDATE POSITION ---
                        touchId.lastUpdateTime = time;
                        touchId.position = posNormalized;
                    }

                    continue;
                }

                // --- NEW ---
                if (state == TouchData.State.Remove)
                    continue;

                TouchId freeTouchId = GetFreeTouchFromPool();
                if (freeTouchId == null)
                    continue;

                freeTouchId.id = id;
                freeTouchId.position = posNormalized;
                freeTouchId.lastUpdateTime = time;

#if UNITY_EDITOR
                if (_log)
                    Debug.Log($"TouchData.State.New {freeTouchId.id}");
#endif

                _activeTouches[id] = freeTouchId;

                freeTouchId.touch.Enable();
                freeTouchId.touch.SetPosition(TouchNorm2Coords(posNormalized));
            }
        }

        private void RemoveTouchFromActive(TouchId touchId, string id)
        {
#if UNITY_EDITOR
            if (_log)
                Debug.Log($"TouchData.State.Remove {touchId.id}");
#endif

            touchId.touch.Disable();
            touchId.ResetValues();

            _activeTouches.Remove(id);
        }

        private TouchId GetFreeTouchFromPool()
        {
            foreach (var touchId in _touchesPool)
            {
                if (string.IsNullOrEmpty(touchId.id))
                    return touchId;
            }

            return null;
        }

        private void Update()
        {
            RemoveOutdatedTouches();

            UpdateTouchesPosition();
        }

        private void UpdateTouchesPosition()
        {
            foreach (var pair in _activeTouches)
            {
                TouchId touchId = pair.Value;
                touchId.touch.SetPositionSmooth(TouchNorm2Coords(touchId.position));
            }
        }

        private const float timeoutTouch = 0.5f;

        private void RemoveOutdatedTouches()
        {
            float time = Time.time;
            var keysToRemove = new List<string>(); // Список для хранения ключей на удаление

            foreach (var pair in _activeTouches)
            {
                TouchId touchId = pair.Value;
                string id = pair.Key;

                if (time - touchId.lastUpdateTime > timeoutTouch)
                    keysToRemove.Add(id);
            }

            foreach (string id in keysToRemove)
            {
                if (_activeTouches.TryGetValue(id, out TouchId touchId))
                {
#if UNITY_EDITOR
                    if (_log)
                        print($"Timeout remove: {id}");
#endif
                    RemoveTouchFromActive(touchId, id);
                }
            }
        }

        private Vector2 TouchNorm2Coords(TouchPoint position) => (Vector2)(position * _rectSize);
    }
}
