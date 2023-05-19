using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewer : MonoBehaviour {

    [Header("Components")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Image sampledColorImage;

    [Header("Events")]
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Awake() {
        closeButton.onClick.AddListener(() => onDisableSampling.Raise(closeButton, null));
    }

    public void SetSampleColor(Component sender, object data) {
        sampledColorImage.color = (Color) data;
    }
    
}
