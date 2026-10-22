using Hammer.MetaWrappers;
using UnityEngine;

namespace Hammer.Nail
{
    [RequireComponent(typeof(Rigidbody), typeof(GrabbableWrapper))]
    public class Nail : MonoBehaviour
    {
        [SerializeField] private float length = 0.073f;
        [SerializeField] private float drivePerJoule = 0.00487f;
        [SerializeField] private float minImpactEnergy = 0.25f;
        [SerializeField] private float maxImpactEnergy = 8.0f;
        [SerializeField] private Collider collider;
        
        private Rigidbody rb;
        private GrabbableWrapper grabbable;
        private float drivenDistance;
        private bool isAnchored;

        public Vector3 DriveDirection => -transform.up;
        public bool IsFullyDriven => drivenDistance >= length;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabbable = GetComponent<GrabbableWrapper>();
        }

        public void Hit(float energy, Nailable target)
        {
            if (IsFullyDriven)
                return;

            if (!isAnchored)
                Anchor(target);
            
            energy = Mathf.Clamp(energy, minImpactEnergy, maxImpactEnergy);
            var distance = Mathf.Min(energy * drivePerJoule, length - drivenDistance);

            transform.position += DriveDirection * distance;
            drivenDistance += distance;
        }

        private void Anchor(Nailable target)
        {
            isAnchored = true;

            grabbable.ForceDrop();
            grabbable.DisableGrab();

            collider.isTrigger = true;
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;

            transform.SetParent(target.transform, true);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.darkRed;
            Gizmos.DrawLine(transform.position, transform.position + -DriveDirection * length);
        }
    }
}