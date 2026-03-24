using UnityEngine;
using UnityEngine.UI;

namespace Bubbles
{
    [RequireComponent(typeof(Button))]
    public class SimpleTouchable : MonoBehaviour, ITouchable
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public bool OnTouch()
        {
            button.onClick.Invoke();
            return true;
        }
    }
}
