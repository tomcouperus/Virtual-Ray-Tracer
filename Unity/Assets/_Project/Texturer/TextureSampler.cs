using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer), typeof(TextureManager))]
public class TextureSampler : MonoBehaviour{

    [SerializeField]
    private Camera mainCamera;

    public Texture2D Texture {
        get {
            Renderer renderer = GetComponent<Renderer>();
            Texture2D texture = renderer.material.mainTexture as Texture2D;
            return texture;
        }
    }

    [SerializeField]
    private SamplingMode _mode;
    public SamplingMode Mode {
        get {return _mode;}
        set {
            _mode = value;
            onSamplingModeChanged.Raise(this, _mode);
            if (Texture) Texture.filterMode = filterMode;
        }
    }
    
    private FilterMode filterMode {
        get {
            return Mode switch {
                SamplingMode.Point => FilterMode.Point,
                SamplingMode.Bilinear => FilterMode.Bilinear,
                _ => FilterMode.Trilinear,
            };
        }
    }

    [SerializeField]
    private bool _isSampling;
    public bool IsSampling {
        get {return _isSampling;}
        set {_isSampling = value;}
    }

    [Header("Events")]
    [SerializeField]
    private GameEvent onSamplingModeChanged;
    [SerializeField]
    private GameEvent onEnableSampling;
    [SerializeField]
    private GameEvent onDisableSampling;
    [SerializeField]
    private GameEvent onTextureSampled;

    [Header("Tutorial events")]
    [SerializeField]
    private UnityEvent OnMouseOverEvent;

    private void Start() {
        Mode = _mode;
        if (IsSampling) onEnableSampling.Raise(this, null);
        else onDisableSampling.Raise(this, null);
    }

    public Color SampleTexture(Vector2 uv, SamplingMode mode) {
        if (!Texture) return Color.white;
        Color color = Color.black;
        switch (mode) {
            case SamplingMode.Point:
                color = Texture.GetPixel((int) (uv.x*Texture.width), (int) (uv.y*Texture.height));
                break;
            case SamplingMode.Bilinear:
                Vector2 sampleUV = uv - new Vector2(0.5f/Texture.width, 0.5f/Texture.height);
                color = Texture.GetPixelBilinear(sampleUV.x, sampleUV.y);
                break;
        }
        return color;
    }

    private void OnMouseOver() {
        Sample();
    }

    private void Sample() {
        if (!IsSampling || !Texture) return;
        OnMouseOverEvent.Invoke();
        
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        Vector2 uv = hit.textureCoord;
        Color color = SampleTexture(uv, Mode);

        SampleData data = new SampleData(color);
        if (Mode == SamplingMode.Bilinear) {
            // Get the pixel coords that the bilinear sample is taken from
            Vector2Int textureSize = new Vector2Int(Texture.width, Texture.height);
            Vector2 uvPos = uv * textureSize;
            Vector2Int pos = Vector2Int.FloorToInt(uvPos);
            
            int xNeighbor = (pos.x == 0 || (pos.x < textureSize.x-1 && Mathf.RoundToInt(uvPos.x) > pos.x)) ? pos.x+1 : pos.x-1;
            int yNeighbor = (pos.y == 0 || (pos.y < textureSize.y-1 && Mathf.RoundToInt(uvPos.y) > pos.y)) ? pos.y+1 : pos.y-1;

            Vector2Int minPos = new Vector2Int(Mathf.Min(pos.x, xNeighbor), Mathf.Min(pos.y, yNeighbor));
            Vector2Int maxPos = new Vector2Int(Mathf.Max(pos.x, xNeighbor), Mathf.Max(pos.y, yNeighbor));

            // Get the colors so that they're ordered like:
            // a, b
            // c, d
            Vector2Int a = new Vector2Int(minPos.x, minPos.y);
            Vector2Int b = new Vector2Int(maxPos.x, minPos.y);
            Vector2Int c = new Vector2Int(minPos.x, maxPos.y);
            Vector2Int d = new Vector2Int(maxPos.x, maxPos.y);

            Color[,] sampledColors = new Color[2,2];
            sampledColors[0,0] = Texture.GetPixel(a.x, a.y);
            sampledColors[1,0] = Texture.GetPixel(b.x, b.y);
            sampledColors[0,1] = Texture.GetPixel(c.x, c.y);
            sampledColors[1,1] = Texture.GetPixel(d.x, d.y);
            data.sampledColors = sampledColors;

            // Get a local uv space between minPos and maxPos for a hit marker in the preview.
            // The offsetting for maxPos and uvPos happens because of the edges not playing nice otherwise.
            float markerUVx = Mathf.InverseLerp(
                minPos.x, 
                maxPos.x + (minPos.x == 0 ? 0.5f : 0) + (maxPos.x == textureSize.x-1 ? 0.5f : 0), 
                uvPos.x - (minPos.x > 0 ? 0.5f : 0));
            float markerUVy = Mathf.InverseLerp(
                minPos.y, 
                maxPos.y + (minPos.y == 0 ? 0.5f : 0) + (maxPos.y == textureSize.y-1 ? 0.5f : 0), 
                uvPos.y - (minPos.y > 0 ? 0.5f : 0));
            data.markerUV = new Vector2(markerUVx, markerUVy);

            // TODO optimize this by moving it into OnMouseEnter and firing with an event or something like that.
            data.textureSprite = TextureManager.CreateTexturePreview(Texture);
            data.textureSize = textureSize;
            data.zoneMarkerUV = (Vector2)minPos / textureSize;
        }
        onSamplingModeChanged.Raise(this, Mode);
        onTextureSampled.Raise(this, data);
    }
}

public enum SamplingMode : int {Point, Bilinear};

public struct SampleData {
    public Color color;
    
    public Color[,] sampledColors;
    public Vector2 markerUV;
    
    public Sprite textureSprite;
    public Vector2Int textureSize;
    public Vector2 zoneMarkerUV;

    public SampleData(Color color) {
        this.color = color;
        this.sampledColors = null;
        markerUV = Vector2.one / 2f;
        textureSprite = null;
        textureSize = Vector2Int.zero;
        zoneMarkerUV = Vector2Int.zero;
    }
}
