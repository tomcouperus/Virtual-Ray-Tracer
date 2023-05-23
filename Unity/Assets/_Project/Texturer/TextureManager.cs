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
    private List<ProceduralTexture> proceduralTextures;

    public int ProceduralTextureCount {
        get {return proceduralTextures.Count;}
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

    public List<Sprite> CreateProceduralTexturePreviews() {
        List<Sprite> sprites = new List<Sprite>();
        foreach (ProceduralTexture prodTex in proceduralTextures) {
            sprites.Add(CreateTexturePreview(prodTex.CreateTexture()));
        }
        return sprites;
    }

    public Texture2D SelectTexture(int index) {
        Texture2D tex = textures[index];
        Texture2D copy = new Texture2D(tex.width, tex.height);
        copy.name = tex.name + "(copy)";
        copy.SetPixels(tex.GetPixels());
        copy.wrapMode = TextureWrapMode.Clamp;
        copy.Apply();
        return copy;
    }

    public Texture2D SelectProceduralTexture(int index) {
        ProceduralTexture tex = proceduralTextures[index];
        return tex.CreateTexture();
    }

    public ProceduralTexture GetProceduralTexture(int index) {
        return proceduralTextures[index];
    }
}
