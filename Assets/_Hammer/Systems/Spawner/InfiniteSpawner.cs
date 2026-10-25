using System.Collections;
using Hammer.MetaWrappers;
using UnityEngine;

namespace Hammer
{
    public class InfiniteSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool stopMotionOnSpawn;
        [SerializeField] private float spawnDelay = 1.5f;

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            var item = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, transform);
            var grabbable = item.GetComponent<GrabbableWrapper>();
            var rigidBody = item.GetComponent<Rigidbody>();

            grabbable.OnGrab += HandleGrab;

            if (stopMotionOnSpawn)
            {
                rigidBody.isKinematic = true;
                // rigidBody.interpolation = RigidbodyInterpolation.None;
            }
        }

        private void HandleGrab(GrabbableWrapper item)
        {
            item.OnGrab -= HandleGrab;

            // if (stopMotionOnSpawn)
            // {
            //     var rigidBody = item.GetComponent<Rigidbody>();
            //     rigidBody.isKinematic = false;
            //     rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            // }

            StartCoroutine(SpawnDelayed());
        }

        private IEnumerator SpawnDelayed()
        {
            yield return new WaitForSeconds(spawnDelay);
            Spawn();
        }
    }
}