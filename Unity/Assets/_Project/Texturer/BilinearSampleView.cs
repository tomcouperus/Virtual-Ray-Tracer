using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SampleView for the bilinear interpolation sampling method.
/// </summary>
public class BilinearSampleView : SampleViewer.SampleView {

    [Header("Components")]
    [SerializeField]
    private Image fullTexture;
    [SerializeField]
    private Image zoneMarker;
    [SerializeField]
    private RectTransform bilinearSampleTexels;
    [SerializeField]
    private Image sampledColorImage00;
    [SerializeField]
    private Image sampledColorImage10;
    [SerializeField]
    private Image sampledColorImage01;
    [SerializeField]
    private Image sampledColorImage11;
    [SerializeField]
    private Image sampleMarker;
    [SerializeField]
    private Image resultColor;

    public override void SetSample(Component sender, object data) {
        if (!(data is SampleData)) return;
        SampleData sampleData = (SampleData) data;

        if (fullTexture.sprite) Object.Destroy(fullTexture.sprite);
        fullTexture.sprite = sampleData.textureSprite;

        zoneMarker.rectTransform.sizeDelta = fullTexture.rectTransform.sizeDelta / sampleData.textureSize * 2;

        float zoneMarkerInsetX = fullTexture.rectTransform.rect.width * sampleData.zoneMarkerUV.x;
        zoneMarker.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, zoneMarkerInsetX, zoneMarker.rectTransform.rect.width);
        float zoneMarkerInsetY = fullTexture.rectTransform.rect.height * sampleData.zoneMarkerUV.y;
        zoneMarker.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, zoneMarkerInsetY, zoneMarker.rectTransform.rect.height);

        
        sampledColorImage00.color = sampleData.sampledColors[0,0];
        sampledColorImage10.color = sampleData.sampledColors[1,0];
        sampledColorImage01.color = sampleData.sampledColors[0,1];
        sampledColorImage11.color = sampleData.sampledColors[1,1];

        float markerInsetX = bilinearSampleTexels.rect.width * sampleData.markerUV.x;
        sampleMarker.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, markerInsetX, sampleMarker.rectTransform.rect.width);
        float markerInsetY = bilinearSampleTexels.rect.height * sampleData.markerUV.y;
        sampleMarker.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, markerInsetY, sampleMarker.rectTransform.rect.height);
        
        resultColor.color = sampleData.color;
    }
}
