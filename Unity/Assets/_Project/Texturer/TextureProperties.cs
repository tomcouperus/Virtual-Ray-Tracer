using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextureProperties : MonoBehaviour {

    [SerializeField]
    private TextureSelect textureSelectPrefab;

    [SerializeField]
    private TMPro.TextMeshProUGUI texturesHeader;
    [SerializeField]
    private TextureSelect nullTextureSelect;
    [SerializeField]
    private TMPro.TextMeshProUGUI proceduralTexturesHeader;

    private TextureManager textureManager;

    private List<TextureSelect> textureSelects = new List<TextureSelect>();

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
            textureSelect.AddOnClickListener(() => textureManager.SelectTexture(texIndex));

            textureSelects.Add(textureSelect);
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
            proceduralEdit.AddListeners(texture, textureSelect);

            int texIndex = i;
            textureSelect.AddOnClickListener(() => {
                textureManager.SelectProceduralTexture(texIndex);
                texture.OnSelect?.Invoke();
            });

            textureSelects.Add(textureSelect);
        }
    }

    private void Clear() {
        textureSelects.Clear();
        textureSelects.Add(nullTextureSelect);

        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child == texturesHeader.gameObject) continue;
            if (child == nullTextureSelect.gameObject) continue;
            if (child == proceduralTexturesHeader.gameObject) continue;
            Object.Destroy(child);
        }
    }

    public void SelectNoTexture() {
        textureManager.ClearTexture();
    }

    private void AddSelectionHighlights() {
        foreach (TextureSelect texSelect in textureSelects) {
            texSelect.AddOnClickListener(() => {
                foreach (TextureSelect t in textureSelects) {
                    if (t == texSelect) t.Selected = true;
                    else t.Selected = false;
                }
            });
        }

        textureSelects[textureManager.TextureIndex+1].Selected = true;
    }

    public void OnShowTextureProperties(Component sender, object data) {
        if (!(data is TextureManager)) return;
        textureManager = (TextureManager) data;

        Clear();
        
        CreateTextureSelects(textureManager);
        CreateProceduralTextureSelects(textureManager);
        AddSelectionHighlights();
    }
}
