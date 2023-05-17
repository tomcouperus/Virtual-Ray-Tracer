using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureSampler : MonoBehaviour
{
    public TextureManager textureManager;

    [SerializeField]
    private Texture2D _texture;
    public Texture2D Texture {
        get {return _texture;}
        set {
            _texture = value;
            if (_texture) _texture.filterMode = FilterMode;
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = _texture;
        }
    }

    [SerializeField]
    private FilterMode _filterMode;
    public FilterMode FilterMode {
        get {return _filterMode;}
        set {
            _filterMode = value;
            if (Texture) Texture.filterMode = _filterMode;
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
        FilterMode = _filterMode;
    }
    #endif
}
