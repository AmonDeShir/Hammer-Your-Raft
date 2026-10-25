using UnityEngine;

namespace Hammer
{
    public class ToolBeltFollower : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Vector3 offset = new(0f, -0.85f, 0f);
        [SerializeField] private float rotationSpeed = 5f;

        private void LateUpdate()
        {
            transform.position = head.position + new Vector3(0f, offset.y, 0f);

            var forward = Vector3.ProjectOnPlane(head.forward, Vector3.up);

            if (forward.sqrMagnitude < 0.001f)
                return;

            var targetRotation = Quaternion.LookRotation(forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.rotation * new Vector3(offset.x, 0f, offset.z);
        }
    }
}