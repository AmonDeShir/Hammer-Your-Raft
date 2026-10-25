using UnityEngine;

namespace Hammer.Nail
{
    public class NailConnector : MonoBehaviour
    {
        [SerializeField] private LayerMask constructionLayers;
        [SerializeField] private float tipRadius = 0.004f;

        private Construction construction;

        public void Initialize(Construction target)
        {
            construction = target;
        }

        public void CheckConnections(Vector3 origin, Vector3 direction, float distance)
        {
            var hits = Physics.SphereCastAll(origin, tipRadius, direction, distance, constructionLayers, QueryTriggerInteraction.Ignore);

            foreach (var hit in hits)
                TryConnect(hit.collider);

            var overlaps = Physics.OverlapSphere(origin + direction * distance, tipRadius, constructionLayers, QueryTriggerInteraction.Ignore);

            foreach (var collider in overlaps)
                TryConnect(collider);
        }

        private void TryConnect(Collider collider)
        {
            var target = collider.GetComponentInParent<Construction>();

            if (!target || target == construction)
                return;

            construction = construction.Merge(target);
        }
    }
}