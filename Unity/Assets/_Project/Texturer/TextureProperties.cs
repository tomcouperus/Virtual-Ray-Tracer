using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureProperties : MonoBehaviour
{
    [SerializeField]
    private TextureManager textureManager;

    [SerializeField]
    private RectTransform textureSelectContainer;
    [SerializeField]
    private TextureSelect textureSelectPrefab;

    [SerializeField]
    private RectTransform proceduralTextureSelectContainer;
    [SerializeField]
    private GameObject proceduralTextureSelectPrefab;

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
        InitialiseTexturePreviews();
        InitialiseProceduralTexturePreviews();
    }

    private void InitialiseTexturePreviews() {
        List<Sprite> texPreviewList = textureManager.CreateTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            Sprite texPreview = texPreviewList[i];
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, textureSelectContainer.transform);
            textureSelect.SetPreview(texPreview);
            textureSelect.SetName(texPreview.name);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {TextureSampler.Texture = textureManager.SelectTexture(texIndex);});
        }
        float containerHeight = textureSelectContainer.sizeDelta.y;
        RectTransform prefabTransform = textureSelectPrefab.transform as RectTransform;
        float prefabHeight = prefabTransform.sizeDelta.y;
        float spacing = GetComponent<VerticalLayoutGroup>().spacing;
        float newContainerHeight = containerHeight + prefabHeight * texPreviewList.Count + spacing * (texPreviewList.Count-1);
        textureSelectContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newContainerHeight);
    }
    
    private void InitialiseProceduralTexturePreviews() {
        List<Sprite> texPreviewList = textureManager.CreateProceduralTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            Sprite texPreview = texPreviewList[i];
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, proceduralTextureSelectContainer.transform);
            textureSelect.SetPreview(texPreview);
            textureSelect.SetName(texPreview.name);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {TextureSampler.Texture = textureManager.SelectProceduralTexture(texIndex);});
        }
    }

    public void SelectNoTexture() {
        TextureSampler.Texture = null;
    }
}
