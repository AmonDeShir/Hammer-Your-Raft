using Hammer;
using Oculus.Interaction.Locomotion;
using StormBreakers;
using UnityEngine;

public class RaftLaunching : MonoBehaviour
{
    [SerializeField] private WaterInteractionPreparer raft;
    [SerializeField] private GameObject pier;
    [SerializeField] private GameObject button;
    [SerializeField] private LocomotionEventsConnection locomotion;
    [SerializeField] private Transform raftTeleportPoint;

    public bool IsFloating => raft != null && raft.IsFloating;

    public void Launch()
    {
        if (IsFloating)
        {
            return;
        }

        EventManager.Trigger(Consts.Events.GameMode.RAFT_READY);
        pier.SetActive(false);
    }

    public void TeleportToRaft()
    {
        var teleport = new LocomotionEvent(0, raftTeleportPoint.position, LocomotionEvent.TranslationType.Absolute);
        locomotion.HandleLocomotionEvent(teleport);
    }

    public void ReturnToEditing()
    {
        EventManager.Trigger(Consts.Events.GameMode.BUILD_MODE);
        pier.SetActive(true);
    }
}
