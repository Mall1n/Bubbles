using UnityEngine;
using UnityEngine.UI;


namespace Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Touch : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private float _canvasAplhaOrigin;
        private Camera _camera;
        private bool _enabled = false;
        private bool _trackBubbles => GameSettings.GameIsStarted;


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasAplhaOrigin = _canvasGroup.alpha;

            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_enabled)
                return;

            Ray ray = _camera.ScreenPointToRay(this.transform.localPosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (_trackBubbles)
            {
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider != null)
                        {
                            if (hits[i].collider.TryGetComponent<BubbleHit>(out BubbleHit bubble))
                                bubble.OnRayHit();
                        }
                    }
                }
            }
            else
            {
                RaycastHit2D raycast = Physics2D.Raycast(this.transform.position, Vector2.zero);
                if (raycast.collider != null)
                {
                    if (raycast.collider.TryGetComponent<Button>(out Button button))
                    {
                        button.onClick?.Invoke();
                    }
                }

                // RaycastHit[] hits = Physics.RaycastAll(this.transform.position, Vector3.zero);

                // if (hits != null)
                // {
                //     for (int i = 0; i < hits.Length; i++)
                //     {
                //         if (hits[i].collider != null)
                //         {
                //             if (hits[i].collider.TryGetComponent<Button>(out Button button))
                //             {
                //                 button.onClick?.Invoke();
                //             }
                //         }
                //     }
                // }
            }
        }

        public void SetPosition(Vector3 position)
        {
            CanvasSetAlpha(_canvasAplhaOrigin); // add condition ?
            _enabled = true;
            transform.localPosition = position;
        }

        public void SetPositionSmooth(Vector3 position)
        {
            CanvasSetAlpha(_canvasAplhaOrigin);
            _enabled = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * 40.0f);
            // transform.localPosition = Vector3.Lerp(transform.localPosition, position, 0.5f); // alt lerp smooth
        }

        public void Disable()
        {
            _enabled = false;
            CanvasSetAlpha(0);
        }

        private void CanvasSetAlpha(float alpha) => _canvasGroup.alpha = alpha;
    }
}
