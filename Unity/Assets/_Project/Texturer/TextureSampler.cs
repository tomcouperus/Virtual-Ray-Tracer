using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureSampler : MonoBehaviour
{
    [SerializeField]
    private Texture2D texture;

    [SerializeField]
    private FilterMode _filterMode;
    public FilterMode FilterMode {
        get {return _filterMode;}
        set {
            _filterMode = value;
            if (texture) texture.filterMode = _filterMode;
        }
    }

    private void Awake() {
        Renderer renderer = GetComponent<Renderer>();
        texture = renderer.material.mainTexture as Texture2D;
        if (texture) texture.filterMode = _filterMode;
    }

    public Color SampleTexture(Vector2 uv) {
        Renderer renderer = GetComponent<Renderer>();
        switch (texture.filterMode) {
            case FilterMode.Point:
                uv.x *= texture.width;
                uv.y *= texture.height;
                return texture.GetPixel((int) uv.x, (int) uv.y);
            case FilterMode.Bilinear:
                return texture.GetPixelBilinear(uv.x, uv.y);
            default:
                Debug.LogError("Trilinear filtering not supported");
                return Color.white;
        }
    }

    public Sprite CreateTexturePreview() {
        if (!texture) return null;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        FilterMode = _filterMode;
    }
    #endif
}
