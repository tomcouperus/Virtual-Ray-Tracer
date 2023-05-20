using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Event system designed by https://www.youtube.com/watch?v=7_dyDmF0Ktw&t=323s

[CreateAssetMenu()]
public class GameEvent : ScriptableObject {
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(Component sender, object data) {
        for (int i = 0; i < listeners.Count; i++) {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void Register(GameEventListener listener) {
        if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void Deregister(GameEventListener listener) {
        if (listeners.Contains(listener)) listeners.Remove(listener);
    }
}
