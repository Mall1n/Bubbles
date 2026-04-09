using UnityEngine;

namespace Bubbles
{
    [RequireComponent(typeof(Transform))]
    [ExecuteAlways]
    public class TransformEditor : MonoBehaviour
    {
        [SerializeField] private Vector3 lookAt;
        [SerializeField] private Vector2 translateRangeZ;
        [SerializeField] private Vector2 rotateX;


        [ContextMenu("LookAt")]
        public void LookAt()
        {
            this.transform.LookAt(lookAt);
        }
        
        [ContextMenu("Translate")]
        public void Translate()
        {
            this.transform.Translate(new Vector3(0, 0, Random.Range(translateRangeZ.x, translateRangeZ.y)), Space.World);
        }

        [ContextMenu("Rotate")]
        public void Rotate()
        {
            this.transform.Rotate(Random.Range(rotateX.x, rotateX.y), 0, 0);
        }
    }
}
