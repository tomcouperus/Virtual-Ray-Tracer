using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewer : MonoBehaviour {

    public abstract class SampleView : MonoBehaviour {
        public abstract void SetSample(Component sender, object data);
    }

    [Header("Components")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private SampleView pointSampleView;

    [SerializeField]
    private SampleView bilinearSampleView;

    [Header("Events")]
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Awake() {
        closeButton.onClick.AddListener(() => onDisableSampling.Raise(closeButton, null));
    }

    private void ActivateMode(SamplingMode mode) {
        bool value = mode == SamplingMode.Point;
        
        pointSampleView.gameObject.SetActive(value);
        bilinearSampleView.gameObject.SetActive(!value);
    }

    public void ActivateMode(Component sender, object data) {
        if (!(data is SamplingMode)) return;
        ActivateMode((SamplingMode) data);
    }
}
