using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomEvent : UnityEvent<Component, object> {}
/// <summary>
/// Observer object in the observer pattern.
/// Set a response event in the editor to make the listener do something on the given GameEvent.
/// 
/// Event system designed by https://www.youtube.com/watch?v=7_dyDmF0Ktw&t=323s
/// </summary>
public class GameEventListener : MonoBehaviour {
    [SerializeField]
    [Tooltip("If enabled, deregisters on destroy. If disable, deregisters on disable.")]
    private bool DeregisterOnDestroy;
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField]
    private CustomEvent response;


    private void OnEnable() {
        gameEvent.Register(this);
    }

    private void OnDisable() {
        if (!DeregisterOnDestroy) gameEvent.Deregister(this);
    }

    private void OnDestroy() {
        if (DeregisterOnDestroy) gameEvent.Deregister(this);
    }

    public void OnEventRaised(Component sender, object data) {
        response.Invoke(sender, data);
    }
    
}
