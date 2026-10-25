using System.Collections;
using System.Collections.Generic;
using Hammer.Extensions;
using Hammer.Music;
using UnityEngine;

namespace Hammer
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private string musicFolder = "Music";

        private readonly List<AudioClip> playlist = new();

        private int currentIndex;
        private AudioSource audioSource;
        private AudioClip lastPlayed;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        private IEnumerator Start()
        {
            IMusicLoader loader;

#if UNITY_ANDROID || UNITY_EDITOR
            loader = new ResourcesMusicLoader(musicFolder);
#else
            loader = new FileMusicLoader(musicFolder);
#endif
            yield return StartCoroutine(loader.Load(SetPlaylist));
        }

        private void SetPlaylist(List<AudioClip> clips)
        {
            playlist.Clear();
            playlist.AddRange(clips);
            Shuffle();
        }

        private void Update()
        {
            if (!audioSource.isPlaying)
                PlayNext();
        }

        public void Skip()
        {
            if (playlist.Count == 0)
                return;

            audioSource.Stop();
            PlayNext();
        }

        private void PlayNext()
        {
            if (playlist.Count == 0)
                return;

            if (currentIndex >= playlist.Count)
                Shuffle();

            var clip = playlist[currentIndex++];

            audioSource.clip = clip;
            audioSource.Play();

            lastPlayed = clip;
        }

        private void Shuffle()
        {
            playlist.Shuffle();
            AvoidRepeat();
            currentIndex = 0;
        }

        private void AvoidRepeat()
        {
            if (playlist.Count > 1 && playlist[0] == lastPlayed)
                (playlist[0], playlist[1]) = (playlist[1], playlist[0]);
        }
    }
}