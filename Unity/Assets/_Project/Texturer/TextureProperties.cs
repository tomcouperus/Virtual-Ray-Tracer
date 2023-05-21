using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureProperties : MonoBehaviour
{
    [SerializeField]
    private TextureManager textureManager;
    [SerializeField]
    private RectTransform textureSelectContainer;
    [SerializeField]
    private TextureSelect textureSelectPrefab;

    private TextureSampler _textureSampler;
    public TextureSampler TextureSampler {
        get {return _textureSampler;} 
        set {_textureSampler = value;}
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void Awake() {
        if (!textureManager) throw new System.NullReferenceException("Texture Properties needs a Texture Manager");
        List<Sprite> texPreviewList = textureManager.CreateTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            Sprite texPreview = texPreviewList[i];
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, textureSelectContainer.transform);
            textureSelect.SetPreview(texPreview);
            textureSelect.SetName(texPreview.name);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {TextureSampler.Texture = textureManager.SelectTexture(texIndex);});
        }
    }

    public void SelectNoTexture() {
        TextureSampler.Texture = null;
    }
}
