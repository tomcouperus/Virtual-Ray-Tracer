using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The observed object in the observer pattern.
/// 
/// Event system designed by https://www.youtube.com/watch?v=7_dyDmF0Ktw&t=323s
/// </summary>
[CreateAssetMenu()]
public class GameEvent : ScriptableObject {
    [SerializeField]
    private List<GameEventListener> listeners = new List<GameEventListener>();

    /// <summary>
    /// Activates the event and notifies all listeners.
    /// </summary>
    /// <param name="sender">Component that raised the event.</param>
    /// <param name="data">Data to transmit to all listeners</param>
    public void Raise(Component sender, object data) {
        // Debug.Log("Raise: "+name); // Left this here for easy debugging of events
        for (int i = 0; i < listeners.Count; i++) {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    /// <summary>
    /// Registers a GameEventListener
    /// </summary>
    /// <param name="listener"></param>
    public void Register(GameEventListener listener) {
        if (!listeners.Contains(listener)) {
            // Debug.Log("Register: "+listener.gameObject.name+" to "+name); // Left this here for easy debugging of events
            listeners.Add(listener);
        }
    }

    /// <summary>
    /// Deregisters a GameEventListener
    /// </summary>
    /// <param name="listener"></param>
    public void Deregister(GameEventListener listener) {
        if (listeners.Contains(listener)) {
            // Debug.Log("Deregister: "+listener.gameObject.name+" to "+name); //Left this here for easy debugging of events
            listeners.Remove(listener);
        }
    }
}
