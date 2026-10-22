using Hammer.MetaWrappers;
using UnityEngine;

namespace Hammer
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GrabbableWrapper))]
    public class FreezeOnSleep : MonoBehaviour
    {
        private Rigidbody rb;
        private GrabbableWrapper grabbable;
        private bool isGrabbed;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabbable = GetComponent<GrabbableWrapper>();

            grabbable.OnGrab += HandleGrab;
            grabbable.OnDrop += HandleDrop;
        }

        private void OnDestroy()
        {
            grabbable.OnGrab -= HandleGrab;
            grabbable.OnDrop -= HandleDrop;
        }

        private void FixedUpdate()
        {
            if (isGrabbed || rb.isKinematic || !rb.IsSleeping())
                return;

            rb.isKinematic = true;
        }

        private void HandleGrab(GrabbableWrapper _)
        {
            isGrabbed = true;
            rb.isKinematic = false;
        }

        private void HandleDrop(GrabbableWrapper _)
        {
            isGrabbed = false;
            rb.isKinematic = false;
        }
    }
}