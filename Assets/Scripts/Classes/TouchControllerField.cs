using UnityEngine;


namespace Game
{
    public abstract class TouchControllerField : MonoBehaviour
    {
        [Header("Touch Controller Field")]
        [SerializeField] protected RectTransform rectTransformField;


        protected Vector2 _fieldSize;


        protected virtual void Awake()
        {
            if (rectTransformField == null)
            {
                Log.Write($"{nameof(TouchControllerField)} don't have RectTransform Field Touches. Further execution has been stopped.", LogType.Warning);
                this.enabled = false;
                return;
            }
            else
            {
                _fieldSize = rectTransformField.rect.size;
            }
        }
    }
}
