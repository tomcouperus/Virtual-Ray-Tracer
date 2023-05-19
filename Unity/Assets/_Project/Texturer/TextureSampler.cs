using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private SamplingMode _samplingMode;
    public SamplingMode SamplingMode {
        get {return _samplingMode;}
        set {
            _samplingMode = value;
            if (Texture) Texture.filterMode = filterMode;
        }
    }
    
    private FilterMode filterMode {
        get {
            return SamplingMode switch {
                SamplingMode.Point => FilterMode.Point,
                SamplingMode.Bilinear => FilterMode.Bilinear,
                _ => FilterMode.Trilinear,
            };
        }
    }

    public bool IsSampling;

    [Header("Events")]
    [SerializeField]
    private GameEvent onTextureSampled;
    [SerializeField]
    private GameEvent onEnableSampling;
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Awake() {
        Renderer renderer = GetComponent<Renderer>();
        Texture = renderer.material.mainTexture as Texture2D;

        if (IsSampling) onEnableSampling.Raise(this, null);
        else onDisableSampling.Raise(this, null);
    }

    public Color SampleTexture(Vector2 uv) {
        if (!Texture) return Color.white;
        switch (Texture.filterMode) {
            case FilterMode.Point:
                return Texture.GetPixel((int) (uv.x*Texture.width), (int) (uv.y*Texture.height));
            case FilterMode.Bilinear:
                return Texture.GetPixelBilinear(uv.x, uv.y);
            default:
                Debug.LogError("Trilinear filtering not supported");
                return Color.white;
        }
    }

    public Sprite CreateTexturePreview() {
        if (!Texture) return null;
        return TextureManager.CreateTexturePreview(Texture);
    }

    private void OnMouseOver() {
        Sample();
    }

    private void Sample() {
        if (!IsSampling || !Texture) return;
        
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        Color color = SampleTexture(hit.textureCoord);
        onTextureSampled.Raise(this, color);
    }

    public void SetSamplingActive(bool value) {
        IsSampling = value;
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        Renderer renderer = GetComponent<Renderer>();
        _texture = renderer.sharedMaterial.mainTexture as Texture2D;
        SamplingMode = _samplingMode;
    }
    #endif
}

public enum SamplingMode : int {Point, Bilinear};
