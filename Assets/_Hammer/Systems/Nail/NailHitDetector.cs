using UnityEngine;

namespace Hammer.Nail
{
    public class NailHitDetector : MonoBehaviour
    {
        [SerializeField] private Nail nail;
        [SerializeField] private float minEnergy = 0.25f;
        [SerializeField] private float hitCooldown = 0.1f;

        private float lastHitTime = float.NegativeInfinity;

        private void OnTriggerEnter(Collider other)
        {
            var hammer = other.GetComponentInParent<Hammer>();

            if (!hammer || !hammer.IsHead(other))
                return;
 
            if (Time.time - lastHitTime < hitCooldown)
                return;

            var energy = hammer.CalculateKineticEnergy();

            if (energy < minEnergy)
                return;

            if (!nail.Hit(energy))
                return;

            lastHitTime = Time.time;
        }
    }
}