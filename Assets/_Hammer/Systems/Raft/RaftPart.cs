using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class RaftPart : MonoBehaviour
{
    [field: SerializeField, Min(0.001f)] public float Mass { get; private set; }
    [field: SerializeField] public BoxCollider VolumeCollider { get; private set; }
}
