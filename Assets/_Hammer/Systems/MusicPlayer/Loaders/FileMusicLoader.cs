using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Hammer.Music
{
    public class FileMusicLoader : IMusicLoader
    {
        private readonly string folder;

        public FileMusicLoader(string folder)
        {
            this.folder = folder;
        }

        public IEnumerator Load(Action<List<AudioClip>> onLoaded)
        {
            var clips = new List<AudioClip>();

            var gameFolder = Directory.GetParent(Application.dataPath).FullName;
            var path = Path.Combine(gameFolder, folder);

            Directory.CreateDirectory(path);

            foreach (var file in Directory.GetFiles(path))
            {
                if (!TryGetAudioType(file, out var type))
                    continue;

                using var request = UnityWebRequestMultimedia.GetAudioClip(new Uri(file).AbsoluteUri, type);

                var handler = (DownloadHandlerAudioClip)request.downloadHandler;
                handler.streamAudio = true;

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning($"Failed to load: {file}: {request.error}");
                    continue;
                }

                var clip = DownloadHandlerAudioClip.GetContent(request);
                clip.name = Path.GetFileNameWithoutExtension(file);
                clips.Add(clip);
            }

            onLoaded?.Invoke(clips);
        }

        private static bool TryGetAudioType(string file, out AudioType type)
        {
            switch (Path.GetExtension(file).ToLowerInvariant())
            {
                case ".mp3":
                    type = AudioType.MPEG;
                    return true;
                
                case ".ogg":
                    type = AudioType.OGGVORBIS;
                    return true;
                
                case ".wav":
                    type = AudioType.WAV;
                    return true;
                
                default:
                    type = AudioType.UNKNOWN;
                    return false;
            }
        }
    }
}