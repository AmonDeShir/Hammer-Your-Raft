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

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(spawnDelay);
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
                rigidBody.interpolation = RigidbodyInterpolation.None;
            }
        }

        private void HandleGrab(GrabbableWrapper item)
        {
            item.OnGrab -= HandleGrab;
            item.transform.parent = null;

            if (stopMotionOnSpawn)
            {
                var rigidBody = item.GetComponent<Rigidbody>();
                rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
                rigidBody.isKinematic = false;
            }


            StartCoroutine(SpawnDelayed());
        }

        private IEnumerator SpawnDelayed()
        {
            yield return new WaitForSeconds(spawnDelay);
            Spawn();
        }
    }
}