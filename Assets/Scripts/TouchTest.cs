using System.Collections;
using UnityEngine;


namespace Game
{
    public class TouchTest : TouchControllerField
    {
        [Header("Test touch")]
        [SerializeField] private bool _enabledTestTouch = false;
        [SerializeField] private Vector2 _normolizedPosition = new Vector2(0, 0);
        [SerializeField] private Touch _testTouch;
        

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            if (_testTouch != null)
            {
                if (_enabledTestTouch)
                    _testTouch.SetPosition(new Vector2(_fieldSize.x * _normolizedPosition.x, _fieldSize.y * _normolizedPosition.y));
                else
                    Destroy(_testTouch.gameObject);
            }

            if (_enabledTestTouch)
                StartCoroutine(UpdateTouch());
        }

        private IEnumerator UpdateTouch()
        {
            while (true)
            {
                _testTouch.SetPosition(new Vector2(_fieldSize.x * _normolizedPosition.x, _fieldSize.y * _normolizedPosition.y));

                yield return null;
            }
        }
    }
}
