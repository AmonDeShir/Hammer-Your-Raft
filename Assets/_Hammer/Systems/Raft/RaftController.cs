using System.Collections.Generic;
using StormBreakers;
using UnityEngine;

namespace Hammer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(WaterInteractionPreparer))]
    public class RaftController : MonoBehaviour
    {
        private WaterInteractionPreparer raft;
        private Rigidbody rigidBody;
        private Vector3 buildPosition;
        private Quaternion buildRotation;

        public bool IsFloating => raft != null && raft.IsFloating;

        private void Awake()
        {
            raft = GetComponent<WaterInteractionPreparer>();
            rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventManager.AddListener(Consts.Events.GameMode.RAFT_READY, HandleRaftReady);
            EventManager.AddListener(Consts.Events.GameMode.BUILD_MODE, HandleBuildMode);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(Consts.Events.GameMode.RAFT_READY, HandleRaftReady);
            EventManager.RemoveListener(Consts.Events.GameMode.BUILD_MODE, HandleBuildMode);
        }

        private void HandleRaftReady()
        {
            if (IsFloating)
            {
                return;
            }

            var position = rigidBody.position;
            var rotation = rigidBody.rotation;

            if (!PrepareRaft() || !raft.TryBeginFloating())
                return;

            buildPosition = position;
            buildRotation = rotation;
        }

        private void HandleBuildMode()
        {
            raft.StopFloating();
            
            rigidBody.position = buildPosition;
            rigidBody.rotation = buildRotation;
        }

        private bool PrepareRaft()
        {
            var elements = new List<HullElement>();

            foreach (var part in GetComponentsInChildren<RaftPart>(true))
            {
                if (!part.isActiveAndEnabled || part.GetComponentInParent<Rigidbody>() != rigidBody)
                    continue;

                elements.Add(new HullElement(part.VolumeCollider, part.Mass));
            }

            return raft.Prepare(elements);
        }
    }
}
