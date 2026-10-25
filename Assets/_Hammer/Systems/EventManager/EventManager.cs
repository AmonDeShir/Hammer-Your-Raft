using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hammer
{
    public static class EventManager
    {
        private static readonly Dictionary<string, Action> events = new();

        public static void AddListener(string name, Action callback)
        {
            events.TryGetValue(name, out var listeners);
            events[name] = listeners + callback;
        }

        public static void RemoveListener(string name, Action callback)
        {
            if (events.TryGetValue(name, out var listeners))
                events[name] = listeners - callback;
        }

        public static void Trigger(string name)
        {
            if (events.TryGetValue(name, out var listeners))
                listeners?.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() => events.Clear();
    }
}