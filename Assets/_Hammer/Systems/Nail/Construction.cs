using Hammer.MetaWrappers;
using UnityEngine;

namespace Hammer.Nail
{
    [RequireComponent(typeof(Rigidbody), typeof(ModelReplacer))]
    public class Construction : MonoBehaviour
    {
        [SerializeField] private Transform attached;
        private ModelReplacer replacer;

        public bool IsRaft => GetComponent<RaftRoot>() != null;

        private void Awake()
        {
            replacer = GetComponent<ModelReplacer>();
        }

        public Construction Merge(Construction other)
        {
            if (!other || other == this)
                return this;

            if (other.IsRaft && !IsRaft)
                return other.Merge(this);

            other.AttachTo(this);
            
            return this;
        }

        private void AttachTo(Construction target)
        {
            if (TryGetComponent<GrabbableWrapper>(out var grab))
            {
                grab.ForceDrop();
                grab.DisableGrab();
            }

            ReconnectChildren(target);
            replacer.Replace(target.attached);
        }

        private void ReconnectChildren(Construction target)
        {
            while (attached.childCount > 0)
                attached.GetChild(0).SetParent(target.attached, true);
        }

        public void Attach(Transform item)
        {
            item.SetParent(attached, true);
        }
    }
}