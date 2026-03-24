using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Bubbles
{
    [RequireComponent(typeof(RectTransform))]
    public class Touch : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private Canvas _canvas;
        private Camera _uiCamera;

        private const float _speedSmooth = 10f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(Canvas canvas)
        {
            _canvas = canvas;
            _uiCamera = _canvas.worldCamera;
        }

        private void LateUpdate()
        {
            Vector2 touchPos = GetScreenPosition();

            if (TryTouchUITouchable(touchPos))
                return;

            TryTouch3DObject(touchPos);
        }

        private Vector2 GetScreenPosition()
        {
            Vector3 worldPoint = _rectTransform.TransformPoint(_rectTransform.rect.center);
            return RectTransformUtility.WorldToScreenPoint(_uiCamera, worldPoint);
        }

        private bool TryTouchUITouchable(Vector2 touchPos)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = touchPos
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                ITouchable touchable = result.gameObject.GetComponent<ITouchable>();
                if (touchable != null)
                {
                    bool onTouched = touchable.OnTouch();
                    if (onTouched) return true;
                }
            }

            return false;
        }

        private bool TryTouch3DObject(Vector2 touchPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                ITouchable touchable = hit.collider.GetComponent<ITouchable>();
                if (touchable != null)
                {
                    bool onTouched = touchable.OnTouch();
                    if (onTouched) return true;
                }
            }

            return false;
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetPositionSmooth(Vector3 position)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * _speedSmooth);
        }

        public void Enable()
        {
            if (!this.gameObject.activeSelf)
                this.gameObject.SetActive(true);
        }

        public void Disable()
        {
            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }
    }
}
