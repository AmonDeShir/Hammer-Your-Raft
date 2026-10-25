using System.Collections;
using StormBreakers;
using UnityEngine;

namespace Hammer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(OceanController))]
    public sealed class OceanStartupRefresh : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float refreshDelay = 1.5f;

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(refreshDelay);

            if (!isActiveAndEnabled || !TryGetComponent(out OceanController oceanController) || !oceanController.isActiveAndEnabled)
            {
                yield break;
            }

            oceanController.enabled = false;
            oceanController.enabled = true;
        }
    }
}
