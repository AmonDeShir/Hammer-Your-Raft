using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hammer.Music
{
    public interface IMusicLoader
    {
        IEnumerator Load(Action<List<AudioClip>> onLoaded);
    }
}