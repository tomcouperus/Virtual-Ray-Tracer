using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TextureManager : ScriptableObject {
    [SerializeField]
    private List<Texture2D> textures;

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
        Texture2D tex = textures[index];
        Texture2D copy = new Texture2D(tex.width, tex.height);
        copy.name = tex.name + "(copy)";
        copy.SetPixels(tex.GetPixels());
        copy.wrapMode = TextureWrapMode.Clamp;
        copy.Apply();
        return copy;
    }
}
