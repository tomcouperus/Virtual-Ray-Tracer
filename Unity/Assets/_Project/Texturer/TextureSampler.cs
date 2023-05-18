using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureSampler : MonoBehaviour{
    public TextureManager textureManager;

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

    private void Awake() {
        Renderer renderer = GetComponent<Renderer>();
        Texture = renderer.material.mainTexture as Texture2D;
    }

    public Color SampleTexture(Vector2 uv) {
        if (!Texture) return Color.white;
        Renderer renderer = GetComponent<Renderer>();
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

    #if UNITY_EDITOR
    private void OnValidate() {
        SamplingMode = _samplingMode;
    }
    #endif
}

public enum SamplingMode : int {Point, Bilinear};
