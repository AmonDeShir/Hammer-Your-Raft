using Unity.VisualScripting;
using UnityEngine;

namespace Hammer
{
    public class FishManDangerZone : MonoBehaviour
    {
        [SerializeField] private FishMan fishMan;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Consts.Tags.PLAYER))
            {
                fishMan.IsInDanger = true;
            }        
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Consts.Tags.PLAYER))
            {
                fishMan.IsInDanger = false;
            }
        }
    }
}