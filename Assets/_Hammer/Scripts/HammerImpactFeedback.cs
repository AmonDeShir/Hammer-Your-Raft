using UnityEngine;
using UnityEngine.Serialization;

namespace Hammer.Nail
{
    [RequireComponent(typeof(Hammer), typeof(AudioSource))]
    public class HammerImpactFeedback : MonoBehaviour
    {
        [FormerlySerializedAs("_impactClips")]
        [SerializeField] private AudioClip[] impactClips;

        private Hammer hammer;
        private AudioClip lastClip;
        private AudioSource audioSource;

        private void Awake()
        {
            hammer = GetComponent<Hammer>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable() => hammer.NailHit += PlayHitSound;
        private void OnDisable() => hammer.NailHit -= PlayHitSound;

        private void PlayHitSound()
        {
            var index = UnityEngine.Random.Range(0, impactClips.Length);

            if (impactClips.Length > 1 && impactClips[index] == lastClip)
                index = (index + 1) % impactClips.Length;

            var clip = impactClips[index];

            if (!clip)
                return;

            audioSource.PlayOneShot(clip);
            lastClip = clip;
        }
    }
}
