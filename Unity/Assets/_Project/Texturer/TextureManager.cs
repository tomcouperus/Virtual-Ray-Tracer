using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour {
    [SerializeField]
    private List<Texture2D> textures;

    public int TextureCount {
        get {return textures.Count;}
    }

    [SerializeField]
    private bool _isSampling;
    public bool IsSampling {
        get {return _isSampling;}
        set {_isSampling = value;}
    }

    [Header("Events")]
    [SerializeField]
    private GameEvent onEnableSampling;
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Start() {
        if (IsSampling) onEnableSampling.Raise(this, null);
        else onDisableSampling.Raise(this, null);
    }

    public static Sprite CreateTexturePreview(Texture2D texture) {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        sprite.name = texture.name;
        return sprite;
    }

    public List<Sprite> CreateTexturePreviews() {
        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D tex in textures) {
            sprites.Add(CreateTexturePreview(tex));
        }
        return sprites;
    }

    public Texture2D SelectTexture(int index) {
        Debug.Log(index);
        Texture2D tex = textures[index];
        Debug.Log(tex.name);
        Texture2D copy = new Texture2D(tex.width, tex.height);
        copy.name = tex.name + "(copy)";
        copy.SetPixels(tex.GetPixels());
        copy.wrapMode = TextureWrapMode.Clamp;
        copy.Apply();
        return copy;
    }
}
