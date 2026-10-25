using Oculus.Interaction.Locomotion;
using UnityEngine;

namespace Hammer
{
    [RequireComponent(typeof(Collider))]
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private LocomotionEventsConnection locomotion;
        [SerializeField] private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Consts.Tags.PLAYER))
                return;

            var teleport = new LocomotionEvent(0, respawnPoint.position, LocomotionEvent.TranslationType.Absolute);
            locomotion.HandleLocomotionEvent(teleport);
        }
    }
}