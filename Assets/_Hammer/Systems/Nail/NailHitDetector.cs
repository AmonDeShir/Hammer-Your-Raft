using UnityEngine;

namespace Hammer.Nail
{
    public class NailHitDetector : MonoBehaviour
    {
        [SerializeField] private Nail nail;
        [SerializeField] private Transform tip;
        [SerializeField] private float minEnergy = 0.1f;
        [SerializeField] private float contactRadius = 0.008f;
        [SerializeField] private LayerMask nailableLayers;
        
        private float lastHitTime;

        private void OnTriggerEnter(Collider other)
        {
            var hammer = other.GetComponentInParent<Hammer>();

            if (!hammer || !hammer.IsHead(other))
                return;

            if (Time.time - lastHitTime < 0.1f)
                return;

            var energy = hammer.CalculateKineticEnergy();
            Debug.Log($"Nail hit with {energy} joules");
            
            if (energy < minEnergy)
                return;
            
            var colliders = Physics.OverlapSphere(tip.position, contactRadius, nailableLayers, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                if (!collider.TryGetComponent(out Nailable target))
                    continue;

                lastHitTime = Time.time;
                nail.Hit(energy, target);
                
                break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(tip.position, contactRadius);
        }
    }
}