using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleViewer : MonoBehaviour {

    [Header("Components")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Image pointSampleImage;
    [SerializeField]
    private Image sampledColorImage00;
    [SerializeField]
    private Image sampledColorImage10;
    [SerializeField]
    private Image sampledColorImage01;
    [SerializeField]
    private Image sampledColorImage11;
    [SerializeField]
    private Image arrow;
    [SerializeField]
    private Image bilinearSampleImage;

    [Header("Events")]
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Awake() {
        closeButton.onClick.AddListener(() => onDisableSampling.Raise(closeButton, null));
    }

    private void ActivateMode(SamplingMode mode) {
        bool value = mode == SamplingMode.Point;
        
        pointSampleImage.gameObject.SetActive(value);
        
        sampledColorImage00.gameObject.SetActive(!value);
        sampledColorImage10.gameObject.SetActive(!value);
        sampledColorImage01.gameObject.SetActive(!value);
        sampledColorImage11.gameObject.SetActive(!value);
        arrow.gameObject.SetActive(!value);
        bilinearSampleImage.gameObject.SetActive(!value);
    }

    public void ActivateMode(Component sender, object data) {
        if (!(data is SamplingMode)) return;
        ActivateMode((SamplingMode) data);
    }

    public void SetSample(Component sender, object data) {
        if (!(data is SampleData)) return;
        SampleData sampleData = (SampleData) data;

        if (sampleData.sampledColors == null) {
            pointSampleImage.color = sampleData.color;
        } else {
            sampledColorImage00.color = sampleData.sampledColors[0,0];
            sampledColorImage10.color = sampleData.sampledColors[1,0];
            sampledColorImage01.color = sampleData.sampledColors[0,1];
            sampledColorImage11.color = sampleData.sampledColors[1,1];
            bilinearSampleImage.color = sampleData.color;
        }
    }
    
}
