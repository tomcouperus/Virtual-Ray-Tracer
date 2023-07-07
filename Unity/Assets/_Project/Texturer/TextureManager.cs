using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
/// <summary>
/// Behaviour that manages the Texture2Ds and ProceduralTextures available 
/// to an object.
/// </summary>
public class TextureManager : MonoBehaviour {
    
    [SerializeField]
    [Tooltip("If set to -1, no texture is initialised. If there are n textures and m procedural textures, then [0..n) is the range for textures and [n..n+m) is the range for procedural textures.")]
    [Min(-1)]
    private int _textureIndex = -1;
    public int TextureIndex {
        get {return _textureIndex;}
    }
    
    [SerializeField]
    private List<Texture2D> textures;

    /// <summary>
    /// The amount of Texture2Ds available.
    /// </summary>
    /// <value></value>
    public int TextureCount {
        get {return textures.Count;}
    }

    [SerializeField]
    private List<ProceduralTexture> proceduralTextures;

    /// <summary>
    /// The amount of ProceduralTextures available.
    /// </summary>
    /// <value></value>
    public int ProceduralTextureCount {
        get {return proceduralTextures.Count;}
    }

    /// <summary>
    /// The active texture
    /// </summary>
    /// <value></value>
    private Texture2D texture {
        get {
            Renderer renderer = GetComponent<Renderer>();
            Texture2D texture = renderer.material.mainTexture as Texture2D;
            return texture;
        }
        set {
            bool isProceduralTexture = TextureIndex >= TextureCount && TextureIndex < TextureCount + ProceduralTextureCount;
            if (isProceduralTexture) Object.Destroy(texture);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = value;

            onSetChildTexture?.Raise(this, value);
        }
    }

    public GameEvent onSetChildTexture;

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
            copy.RefreshTextureAction = () => {
                int procIndex = TextureIndex - TextureCount;
                SelectProceduralTexture(procIndex);
            };
            proceduralTexturesCopy.Add(copy);
        }
        proceduralTextures = proceduralTexturesCopy;

        if (_textureIndex < 0) {
            ClearTexture();
        } else if (_textureIndex < TextureCount) {
            SelectTexture(_textureIndex);
        } else {
            SelectProceduralTexture(_textureIndex - TextureCount);
        }
    }

    /// <summary>
    /// Creates a Sprite from a texture to function as a texture preview.
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Sprite CreateTexturePreview(Texture2D texture) {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        sprite.name = texture.name;
        return sprite;
    }

    /// <summary>
    /// Creates Sprites for all textures
    /// </summary>
    /// <returns></returns>
    public List<Sprite> CreateTexturePreviews() {
        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D tex in textures) {
            sprites.Add(CreateTexturePreview(tex));
        }
        return sprites;
    }

    /// <summary>
    /// Creates Sprites for all procedural textures
    /// </summary>
    /// <returns></returns>
    public List<Sprite> CreateProceduralTexturePreviews() {
        List<Sprite> sprites = new List<Sprite>();
        foreach (ProceduralTexture prodTex in proceduralTextures) {
            sprites.Add(CreateTexturePreview(prodTex.CreatePreviewTexture()));
        }
        return sprites;
    }

    /// <summary>
    /// Selects a texture and changes this on the GameObject's Renderer as well.
    /// </summary>
    /// <param name="index"></param>
    public void SelectTexture(int index) {
        texture = textures[index];
        _textureIndex = index;
    }

    /// <summary>
    /// Selects a procedural texture and changes this on the GameObject's Renderer as well.
    /// </summary>
    /// <param name="index"></param>
    public void SelectProceduralTexture(int index) {
        ProceduralTexture tex = proceduralTextures[index];
        texture = tex.CreateTexture();
        _textureIndex = index + TextureCount;
    }

    /// <summary>
    /// Gets a procedural texture by index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ProceduralTexture GetProceduralTexture(int index) {
        return proceduralTextures[index];
    }

    /// <summary>
    /// Clears the active texture
    /// </summary>
    public void ClearTexture() {
        texture = null;
        _textureIndex = -1;
    }

    /// <summary>
    /// Creates a preview of the active texture
    /// </summary>
    /// <returns></returns>
    public Sprite CreateTexturePreview() {
        if (!texture) return null;
        return CreateTexturePreview(texture);
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        if (_textureIndex >= TextureCount + ProceduralTextureCount) _textureIndex = TextureCount + ProceduralTextureCount - 1;
    }
    #endif
}
