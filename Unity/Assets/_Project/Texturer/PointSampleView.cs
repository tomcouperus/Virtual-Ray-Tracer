using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SampleView for the nearest neighbour sampling method.
/// </summary>
public class PointSampleView : SampleViewer.SampleView {
    [SerializeField]
    private Image resultColor;

    public override void SetSample(Component sender, object data) {
        if (!(data is SampleData)) return;
        SampleData sampleData = (SampleData) data;
        resultColor.color = sampleData.color;
    }
}
