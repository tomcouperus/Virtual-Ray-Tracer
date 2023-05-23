using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextureProperties : MonoBehaviour {

    [SerializeField]
    private TextureSelect textureSelectPrefab;
    [SerializeField]
    private TextMeshProUGUI proceduralTexturesHeader;

    private List<int> textureSelectIndices = new List<int>();

    private TextureSampler textureSampler;

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void CreateTextureSelects(TextureManager textureManager) {
        List<Sprite> texPreviewList = textureManager.CreateTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            int siblingIndex = proceduralTexturesHeader.transform.GetSiblingIndex();
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, transform);
            textureSelect.transform.SetSiblingIndex(siblingIndex);
            textureSelectIndices.Add(siblingIndex);
            
            textureSelect.SetPreview(texPreviewList[i]);
            textureSelect.SetName(texPreviewList[i].name);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {textureSampler.Texture = textureManager.SelectTexture(texIndex);});


        }
    }
    
    private void CreateProceduralTextureSelects(TextureManager textureManager) {
        List<Sprite> texPreviewList = textureManager.CreateProceduralTexturePreviews();
        if (texPreviewList.Count == 0) {
            proceduralTexturesHeader.gameObject.SetActive(false);
            return;
        }
        proceduralTexturesHeader.gameObject.SetActive(true);

        for (int i = 0; i < texPreviewList.Count; i++) {
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, transform);
            textureSelect.SetPreview(texPreviewList[i]);
            textureSelect.SetName(texPreviewList[i].name);

            ProceduralTexture texture = textureManager.GetProceduralTexture(i);
            ProceduralTextureEdit proceduralEdit = Instantiate(texture.ProceduralTextureEditPrefab, transform);
            proceduralEdit.SetProceduralValues(texture);
            proceduralEdit.AddListeners(texture);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {
                Texture2D tex = textureManager.SelectProceduralTexture(texIndex);
                textureSampler.Texture = tex;
            });
        }
    }

    private void DestroyTexturePreviews() {
        foreach (int i in textureSelectIndices) {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
        textureSelectIndices.Clear();
    }

    private void DestroyProceduralTexturePreviews() {
        for (int i = proceduralTexturesHeader.transform.GetSiblingIndex()+1; i < transform.childCount; i++) {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SelectNoTexture() {
        textureSampler.Texture = null;
    }

    public void OnShowTextureProperties(Component sender, object data) {
        if (!(data is TextureSampler)) return;
        textureSampler = (TextureSampler) data;
        TextureManager textureManager = textureSampler.textureManager;

        DestroyTexturePreviews();
        DestroyProceduralTexturePreviews();
        
        CreateTextureSelects(textureManager);
        CreateProceduralTextureSelects(textureManager);
    }
}
