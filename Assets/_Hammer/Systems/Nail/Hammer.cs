using System;
using UnityEngine;

namespace Hammer.Nail
{
    [RequireComponent(typeof(Rigidbody))]
    public class Hammer : MonoBehaviour
    {
        [SerializeField] private Transform headPosition;
        [SerializeField] private Collider headCollider;
        [SerializeField] private float headMass = 0.75f;

        private Vector3 previousHeadPosition;
        private Vector3 headDisplacement;

        public event Action NailHit;

        public Vector3 HeadVelocity { get; private set; }

        private void OnEnable()
        {
            previousHeadPosition = headPosition.position;
            headDisplacement = Vector3.zero;
            HeadVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            var currentHeadPosition = headPosition.position;
            headDisplacement = currentHeadPosition - previousHeadPosition;
            HeadVelocity = headDisplacement / Time.fixedDeltaTime;
            previousHeadPosition = currentHeadPosition;
        }

        public bool IsHead(Collider collider)
        {
            return collider == headCollider;
        }

        public void ReportNailHit()
        {
            NailHit?.Invoke();
        }

        public float CalculateKineticEnergy()
        {
            var velocity = HeadVelocity;
            return 0.5f * headMass * velocity.sqrMagnitude;
        }
    }
}
