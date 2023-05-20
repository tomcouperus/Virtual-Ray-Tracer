using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Event system designed by https://www.youtube.com/watch?v=7_dyDmF0Ktw&t=323s

[System.Serializable]
public class CustomEvent : UnityEvent<Component, object> {}
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
        // Debug.Log(gameObject.name + " responds to "+gameEvent.name);
        response.Invoke(sender, data);
    }
    
}
