using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureManager : MonoBehaviour {
    
    [SerializeField]
    [Tooltip("If set to -1, no texture is initialised. Is checked first.")]
    [Min(-1)]
    private int InitialTextureIndex = -1;
    [SerializeField]
    [Tooltip("If set to -1, no procedural texture is initialised. Is checked second.")]
    [Min(-1)]
    private int InitialProceduralTextureIndex = -1;
    
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

    private Texture2D Texture {
        get {
            Renderer renderer = GetComponent<Renderer>();
            Texture2D texture = renderer.material.mainTexture as Texture2D;
            return texture;
        }
        set {
            if (textureIsProcedural) Object.Destroy(Texture);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = value;
        }
    }
    private bool textureIsProcedural;

    private void Awake() {
        List<Texture2D> texturesCopy = new List<Texture2D>();
        for (int i = 0; i < TextureCount; i++) {
            Texture2D tex = Instantiate(textures[i]);
            tex.name = textures[i].name;
            texturesCopy.Add(tex);
        }
        textures = texturesCopy;

        List<ProceduralTexture> proceduralTexturesCopy = new List<ProceduralTexture>();
        for (int i = 0; i < ProceduralTextureCount; i++) {
            ProceduralTexture copy = Instantiate(proceduralTextures[i]);
            proceduralTexturesCopy.Add(copy);
        }
        proceduralTextures = proceduralTexturesCopy;

        if (InitialTextureIndex >= 0 && InitialTextureIndex < TextureCount) SelectTexture(InitialTextureIndex);
        else if (InitialProceduralTextureIndex >= 0 && InitialProceduralTextureIndex < ProceduralTextureCount) SelectProceduralTexture(InitialProceduralTextureIndex);
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
            sprites.Add(CreateTexturePreview(prodTex.CreatePreviewTexture()));
        }
        return sprites;
    }

    public void SelectTexture(int index) {
        Texture = textures[index];
        textureIsProcedural = false;
    }

    public void SelectProceduralTexture(int index) {
        ProceduralTexture tex = proceduralTextures[index];
        Texture = tex.CreateTexture();
        textureIsProcedural = true;
    }

    public ProceduralTexture GetProceduralTexture(int index) {
        return proceduralTextures[index];
    }

    public void ClearTexture() {
        Texture = null;
    }

    public Sprite CreateTexturePreview() {
        if (!Texture) return null;
        return CreateTexturePreview(Texture);
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        if (InitialTextureIndex >= TextureCount) InitialTextureIndex = TextureCount-1;
        if (InitialProceduralTextureIndex >= ProceduralTextureCount) InitialProceduralTextureIndex = ProceduralTextureCount-1;
    }
    #endif
}
