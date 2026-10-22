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

        private Rigidbody rigidBody;
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        public bool IsHead(Collider collider)
        {
            return collider == headCollider;
        }
        
        public float CalculateKineticEnergy()
        {
            var velocity = rigidBody.GetPointVelocity(headPosition.position);
            
            // Ek = 1/2mv^2
            return 0.5f * headMass * velocity.sqrMagnitude;
        }
    }
}