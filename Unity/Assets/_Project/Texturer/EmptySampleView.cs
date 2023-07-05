using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SampleView for when the SampleViewer is active, but no sampling object is hovered on.
/// Essentially displays nothing
/// </summary>
public class EmptySampleView : SampleViewer.SampleView {
    public override void SetSample(Component sender, object data) {}
}
