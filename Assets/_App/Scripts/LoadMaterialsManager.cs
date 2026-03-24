using System.Collections;
using UnityEngine;

namespace Bubbles
{
    public class LoadMaterialsManager : MonoBehaviour
    {
        private Transform[] gos;

        private void Start()
        {
            gos = this.gameObject.GetComponentsInChildren<Transform>(true);

            StartCoroutine(LoadMaterials());
        }

        private IEnumerator LoadMaterials()
        {
            yield return null;

            foreach (var go in gos)
            {
                go.gameObject.SetActive(true);
            }

            yield return null;

            Destroy(this.gameObject);
        }
    }
}
