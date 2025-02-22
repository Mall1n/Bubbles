using System.Collections;
using UnityEngine;


namespace Game
{
    public class TouchesLidarPool : TouchControllerField
    {
        [Header("Settings Pool")]
        [SerializeField] private short _poolObjectCount = 5;
        [SerializeField] private Touch _targetObject;
        [SerializeField] private LidarTouchesHandler _targetLidarTouches;

        // #if UNITY_EDITOR

        // #endif

        private Touch[] transformsPool;


        protected override void Awake()
        {
            base.Awake();

            InitObjectPool();

            if (transformsPool != null && transformsPool.Length > 0)
                StartCoroutine(UpdateImageTouches());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void InitObjectPool()
        {
            if (_targetObject != null)
            {
                transformsPool = new Touch[_poolObjectCount];

                for (int i = 0; i < _poolObjectCount; i++)
                {
                    transformsPool[i] = Instantiate<Touch>(_targetObject, this.transform);
                }
            }
        }

        // private void DisableTouches()
        // {
        //     for (short i = 0; i < _targetLidarTouches.Touches.Count; i++)
        //     {
        //         transformsPool[i].Disable();
        //     }
        // }


        // Sometimes update may miss a OSCMessageCommand.Create and will read it as a OSCMessageCommand.Update
        private IEnumerator UpdateImageTouches()
        {
            while (true)
            {
                UpdateTouchesOnField();

                yield return null;
            }
        }

        private void UpdateTouchesOnField()
        {
            short countToches = _targetLidarTouches.Touches.Count > _poolObjectCount ? _poolObjectCount : (short)_targetLidarTouches.Touches.Count;

            for (short i = 0; i < countToches; i++)
            {
                if (_targetLidarTouches.Touches[i].command == OSCMessageCommand.Create)
                    transformsPool[i].SetPosition(TouchNorm2Coords(_targetLidarTouches.Touches[i]));
                else
                    transformsPool[i].SetPositionSmooth(TouchNorm2Coords(_targetLidarTouches.Touches[i]));
            }

            for (short i = countToches; i < _poolObjectCount; i++)
            {
                transformsPool[i].Disable();
            }

        }

        private Vector3 TouchNorm2Coords(TouchData touch) => (Vector3)(touch.normalizedTouchPoint * _fieldSize);


    }
}
