using UnityEngine;

namespace Hammer
{
    public class ModelReplacer : MonoBehaviour
    {
        [SerializeField] private GameObject model;

        public GameObject Replace(Transform parent = null)
        {
            var replacement = Instantiate(model, transform.position, transform.rotation, parent ? parent : transform.parent);

            Destroy(gameObject);
            
            return replacement;
        }
    }
}