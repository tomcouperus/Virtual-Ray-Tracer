using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextureProperties : MonoBehaviour {

    [SerializeField]
    private TextureSelect textureSelectPrefab;

    [SerializeField]
    private GameObject texturesHeader;
    [SerializeField]
    private GameObject nullTextureSelect;
    [SerializeField]
    private GameObject proceduralTexturesHeader;
    [SerializeField]
    private GameObject UVProjectionHeader;

    private TextureManager textureManager;

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void CreateTextureSelects(TextureManager textureManager) {
        List<Sprite> texPreviewList = textureManager.CreateTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            int siblingIndex = proceduralTexturesHeader.transform.GetSiblingIndex();
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, transform);
            textureSelect.transform.SetSiblingIndex(siblingIndex);
            
            textureSelect.SetPreview(texPreviewList[i]);
            textureSelect.SetName(texPreviewList[i].name);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {textureManager.SelectTexture(texIndex);});


        }
    }
    
    private void CreateProceduralTextureSelects(TextureManager textureManager) {
        List<Sprite> texPreviewList = textureManager.CreateProceduralTexturePreviews();
        if (texPreviewList.Count == 0) {
            proceduralTexturesHeader.SetActive(false);
            return;
        }
        proceduralTexturesHeader.SetActive(true);

        for (int i = 0; i < texPreviewList.Count; i++) {
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, transform);
            textureSelect.SetPreview(texPreviewList[i]);
            textureSelect.SetName(texPreviewList[i].name);

            ProceduralTexture texture = textureManager.GetProceduralTexture(i);
            ProceduralTextureEdit proceduralEdit = Instantiate(texture.ProceduralTextureEditPrefab, transform);
            proceduralEdit.SetProceduralValues(texture);
            proceduralEdit.AddListeners(texture);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {textureManager.SelectProceduralTexture(texIndex);});
        }
    }

    private void Clear() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child == texturesHeader) continue;
            if (child == nullTextureSelect) continue;
            if (child == proceduralTexturesHeader) continue;
            Object.Destroy(child);
        }
    }

    public void SelectNoTexture() {
        textureManager.ClearTexture();
    }

    public void OnShowTextureProperties(Component sender, object data) {
        if (!(data is TextureManager)) return;
        textureManager = (TextureManager) data;

        Clear();
        
        CreateTextureSelects(textureManager);
        CreateProceduralTextureSelects(textureManager);
    }
}
