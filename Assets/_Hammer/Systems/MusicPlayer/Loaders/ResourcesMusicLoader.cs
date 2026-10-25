using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hammer.Music
{
    public class ResourcesMusicLoader : IMusicLoader
    {
        private readonly string folder;

        public ResourcesMusicLoader(string folder)
        {
            this.folder = folder;
        }

        public IEnumerator Load(Action<List<AudioClip>> onLoaded)
        {
            var clips = new List<AudioClip>(Resources.LoadAll<AudioClip>(folder));

            onLoaded?.Invoke(clips);
            
            yield break;
        }
    }
}