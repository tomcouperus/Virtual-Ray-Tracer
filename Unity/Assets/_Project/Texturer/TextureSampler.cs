using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class TextureSampler : MonoBehaviour{

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    public TextureManager textureManager;

    //TODO destroy textures, because they are apparently not collected by the GC.
    [SerializeField]
    private Texture2D _texture;
    public Texture2D Texture {
        get {return _texture;}
        set {
            _texture = value;
            if (_texture) _texture.filterMode = filterMode;
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = _texture;
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

    [Header("Events")]
    [SerializeField]
    private GameEvent onSamplingModeChanged;
    [SerializeField]
    private GameEvent onTextureSampled;

    [Header("Tutorial events")]
    [SerializeField]
    private UnityEvent OnMouseOverEvent;

    private void Awake() {
        Renderer renderer = GetComponent<Renderer>();
        Texture = renderer.material.mainTexture as Texture2D;
    }

    public Color SampleTexture(Vector2 uv, SamplingMode mode) {
        if (!Texture) return Color.white;
        Color color = Color.black;
        switch (mode) {
            case SamplingMode.Point:
                color = Texture.GetPixel((int) (uv.x*Texture.width), (int) (uv.y*Texture.height));
                break;
            case SamplingMode.Bilinear:
                color = Texture.GetPixelBilinear(uv.x, uv.y);
                break;
        }
        return color;
    }

    public Sprite CreateTexturePreview() {
        if (!Texture) return null;
        return TextureManager.CreateTexturePreview(Texture);
    }

    private void OnMouseOver() {
        Sample();
    }

    private void Sample() {
        if (!textureManager.IsSampling || !Texture) return;
        OnMouseOverEvent.Invoke();
        
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        Vector2 uv = hit.textureCoord;
        Color color = SampleTexture(uv, Mode);

        SampleData data = new SampleData(color);
        if (Mode == SamplingMode.Bilinear) {
            // Debug.Log(uv);;
            // Grab the pixels that are used in the bilinear sampling
            int x = Mathf.RoundToInt(uv.x * Texture.width);
            int y = Mathf.RoundToInt(uv.y * Texture.height);

            int xNeighbor = ((x < uv.x && x != 0) || x == Texture.width) ? x-1 : x+1;
            int yNeighbor = ((y < uv.y && y != 0) || y == Texture.height) ? y-1 : y+1;

            // Get the colors in order of:
            // a, b
            // c, d
            Vector2 a = new Vector2(Mathf.Min(x, xNeighbor), Mathf.Min(y, yNeighbor));
            Vector2 b = new Vector2(Mathf.Max(x, xNeighbor), Mathf.Min(y, yNeighbor));
            Vector2 c = new Vector2(Mathf.Min(x, xNeighbor), Mathf.Max(y, yNeighbor));
            Vector2 d = new Vector2(Mathf.Max(x, xNeighbor), Mathf.Max(y, yNeighbor));

            Vector2 textureSize = new Vector2(Texture.width, Texture.height);
            Color[,] sampledColors = new Color[2,2];
            sampledColors[0,0] = SampleTexture(a / textureSize, SamplingMode.Point);
            sampledColors[1,0] = SampleTexture(b / textureSize, SamplingMode.Point);
            sampledColors[0,1] = SampleTexture(c / textureSize, SamplingMode.Point);
            sampledColors[1,1] = SampleTexture(d / textureSize, SamplingMode.Point);

            data.sampledColors = sampledColors;
        }

        onTextureSampled.Raise(this, data);
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        Renderer renderer = GetComponent<Renderer>();
        _texture = renderer.sharedMaterial.mainTexture as Texture2D;
        Mode = _mode;
    }
    #endif
}

public enum SamplingMode : int {Point, Bilinear};

public struct SampleData {
    public Color color;
    public Color[,] sampledColors;

    public SampleData(Color color) {
        this.color = color;
        this.sampledColors = null;
    }
}
