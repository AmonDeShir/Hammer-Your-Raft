using Hammer.MetaWrappers;
using UnityEngine;

namespace Hammer.Nail
{
    [RequireComponent(typeof(Rigidbody), typeof(GrabbableWrapper), typeof(ModelReplacer))]
    public class Nail : MonoBehaviour
    {
        [SerializeField] private Transform tip;
        [SerializeField] private Collider nailCollider;
        [SerializeField] private NailConnector connector;
        [SerializeField] private LayerMask constructionLayers;

        [SerializeField] private float length = 0.073f;
        [SerializeField] private float drivePerJoule = 0.00487f;
        [SerializeField] private float maxImpactEnergy = 8f;
        [SerializeField] private float contactRadius = 0.008f;

        private Rigidbody rb;
        private GrabbableWrapper grabbable;
        private ModelReplacer replacer;
        private float drivenDistance;
        private bool isAnchored;

        public Vector3 DriveDirection => -transform.up;
        public bool IsFullyDriven => drivenDistance >= length;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabbable = GetComponent<GrabbableWrapper>();
            replacer = GetComponent<ModelReplacer>();
        }

        public bool Hit(float energy)
        {
            if (IsFullyDriven)
                return false;

            if (!isAnchored && !TryAnchor())
                return false;

            energy = Mathf.Min(energy, maxImpactEnergy);
            var distance = Mathf.Min(energy * drivePerJoule, length - drivenDistance);
            
            Drive(distance);

            if (IsFullyDriven)
                replacer.Replace();

            return true;
        }

        private bool TryAnchor()
        {
            var colliders = Physics.OverlapSphere(tip.position, contactRadius, constructionLayers, QueryTriggerInteraction.Ignore);

            foreach (var collider in colliders)
            {
                var target = collider.GetComponentInParent<Construction>();

                if (!target)
                    continue;

                Anchor(target);
                
                return true;
            }

            return false;
        }

        private void Anchor(Construction target)
        {
            grabbable.ForceDrop();
            grabbable.DisableGrab();

            nailCollider.isTrigger = true;

            rb.isKinematic = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;

            target.Attach(transform);
            connector.Initialize(target);

            isAnchored = true;
        }

        private void Drive(float distance)
        {
            connector.CheckConnections(tip.position, DriveDirection, distance);

            transform.position += DriveDirection * distance;
            drivenDistance += distance;
        }

        private void OnDrawGizmosSelected()
        {
            if (!tip)
                return;
 
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(tip.position, contactRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(tip.position, tip.position + DriveDirection * length);
        }
    }
}