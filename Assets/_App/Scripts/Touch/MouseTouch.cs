using UnityEngine;

namespace Bubbles
{
    public class MouseTouch : MonoBehaviour
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Touch _mouseTouch;

        private RectTransform _mouseTouchRect;
        private RectTransform _mouseParentRect;
        private bool _isMouseDown = false;

        private void Awake()
        {
            _mouseTouchRect = _mouseTouch.GetComponent<RectTransform>();
            _mouseParentRect = _mouseTouchRect.parent as RectTransform;
            _mouseTouch.Init(_canvas);

            _mouseTouch.Disable();
        }

        private void Update()
        {
            if (!_enabled)
                return;

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Input.mousePosition;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _mouseParentRect,
                    mousePos,
                    _canvas.worldCamera,
                    out Vector2 localpos
                );

                _mouseTouchRect.localPosition = localpos;

                if (!_isMouseDown)
                {
                    _mouseTouch.Enable();
                    _isMouseDown = true;
                }
            }
            else if (_isMouseDown)
            {
                _mouseTouch.Disable();
                _isMouseDown = false;
            }
        }
    }
}
