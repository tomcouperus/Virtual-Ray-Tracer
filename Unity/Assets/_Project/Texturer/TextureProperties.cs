using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Project.UI.Scripts.Control_Panel;

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

    [SerializeField]
    private FloatEdit transitionEdit;

    [SerializeField]
    private BoolEdit loopEdit;

    [SerializeField]
    private TMP_Dropdown objectDropdown;

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

    private void CreateUVProjectionSelects(TextureManager textureManager) {
        if(!textureManager.ActiveChild.HasValue) return;

        if(textureManager.transform.GetChild(textureManager.ActiveChild.Value).GetComponent<Renderer>().material.name == "UVProjection (Instance)") {
            UVProjectionHeader.SetActive(true);
            transitionEdit.gameObject.SetActive(true);
            loopEdit.gameObject.SetActive(true);
        } else {
            UVProjectionHeader.SetActive(false);
            transitionEdit.gameObject.SetActive(false);
            loopEdit.gameObject.SetActive(false);
            return;
        }
        
        transitionEdit.OnValueChanged.AddListener((value) => { textureManager.Transition = value; });
        loopEdit.OnValueChanged.AddListener((value) => { textureManager.Loop = value; });

        if (textureManager.ChildCount > 1){
            objectDropdown.gameObject.SetActive(true);
            objectDropdown.onValueChanged.AddListener((value) => { textureManager.ActiveChild = value; });
        }
        else
            objectDropdown.gameObject.SetActive(false);
    }

    private void Clear() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child == texturesHeader) continue;
            if (child == nullTextureSelect) continue;
            if (child == proceduralTexturesHeader) continue;
            if (child == UVProjectionHeader) continue;
            if (child == transitionEdit.gameObject) continue;
            if (child == loopEdit.gameObject) continue;
            if (child == objectDropdown.gameObject) continue;
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
        
        CreateUVProjectionSelects(textureManager);
        CreateTextureSelects(textureManager);
        CreateProceduralTextureSelects(textureManager);
    }
}
