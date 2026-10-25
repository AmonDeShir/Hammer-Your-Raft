using System.Collections;
using UnityEngine;

namespace Hammer
{
    public class FishMan : MonoBehaviour
    {
        private static readonly int DangerID = Animator.StringToHash("Danger");
        private static readonly int WaveID = Animator.StringToHash("Wave");
        private static readonly int YesID = Animator.StringToHash("Yes");
        private static readonly int NoID = Animator.StringToHash("No");

        private static readonly int[] IdleAnimations = { WaveID, YesID, NoID };

        [SerializeField] private Animator animator;
        [SerializeField] private float minIdleInterval = 5f;
        [SerializeField] private float maxIdleInterval = 10f;

        private bool isInDanger = false;
        
        public bool IsInDanger
        {
            get => isInDanger;
            set
            {
                isInDanger = value;
                animator.SetBool(DangerID, isInDanger);
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener(Consts.Events.GameMode.RAFT_READY, Wave);
            EventManager.AddListener(Consts.Events.GameMode.BUILD_MODE, No);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(Consts.Events.GameMode.RAFT_READY, Wave);
            EventManager.RemoveListener(Consts.Events.GameMode.BUILD_MODE, No);
        }

        private void Start()
        {
            StartCoroutine(PlayRandomIdleAnimations());
        }

        public void Wave() => animator.SetTrigger(WaveID);
        public void Yes() => animator.SetTrigger(YesID);
        public void No() => animator.SetTrigger(NoID);

        private IEnumerator PlayRandomIdleAnimations()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minIdleInterval, maxIdleInterval));

                if (IsInDanger)
                    continue;

                var index = Random.Range(0, IdleAnimations.Length);
                animator.SetTrigger(IdleAnimations[index]);
            }
        }
    }
}