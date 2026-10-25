using Hammer;
using StormBreakers;
using UnityEngine;

public class RaftLaunching : MonoBehaviour
{
    [SerializeField] private WaterInteractionPreparer raft;
    [SerializeField] private GameObject pier;
    [SerializeField] private GameObject button;

    public bool IsFloating => raft != null && raft.IsFloating;

    public void Launch()
    {
        if (IsFloating)
        {
            return;
        }

        EventManager.Trigger(Consts.Events.GameMode.RAFT_READY);
        
        pier.SetActive(false);
        button.SetActive(false);
    }

    public void ReturnToEditing()
    {
        EventManager.Trigger(Consts.Events.GameMode.BUILD_MODE);
        
        pier.SetActive(true);
        button.SetActive(true);
    }
}
