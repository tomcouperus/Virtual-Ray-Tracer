using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSampleView : SampleViewer.SampleView {
    [SerializeField]
    private Image resultColor;

    public override void SetSample(Component sender, object data) {
        if (!(data is SampleData)) return;
        SampleData sampleData = (SampleData) data;
        resultColor.color = sampleData.color;
    }
}
