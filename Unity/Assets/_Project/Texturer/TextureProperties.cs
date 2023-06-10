using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _Project.UI.Scripts.Control_Panel;

public class TextureProperties : MonoBehaviour {

    [Header("UV mapping")]
    [SerializeField]
    private GameObject UVProjectionHeader;
    [SerializeField]
    private FloatEdit transitionEdit;

    [SerializeField]
    private BoolEdit loopEdit;

    [SerializeField]
    private TMP_Dropdown objectDropdown;

    [Header("Textures")]
    [SerializeField]
    private TextureSelect textureSelectPrefab;

    [SerializeField]
    private GameObject texturesHeader;
    [SerializeField]
    private GameObject nullTextureSelect;
    [SerializeField]
    private GameObject proceduralTexturesHeader;

    [Serializable]
    public class Event : UnityEvent { }
    [Header("Events")]
    public Event onTextureSelected;

    private TextureManager textureManager;
    private TextureProjector textureProjector;

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

    private void CreateUVProjection(TextureProjector textureProjector) {
        onTextureSelected.Invoke();

        UVProjectionHeader.SetActive(true);
        transitionEdit.gameObject.SetActive(true);
        loopEdit.gameObject.SetActive(true);
        
        transitionEdit.OnValueChanged.AddListener((value) => { textureProjector.Transition = value; });
        loopEdit.OnValueChanged.AddListener((value) => { textureProjector.Loop = value; });

        if (textureProjector.ChildCount > 1) {
            objectDropdown.gameObject.SetActive(true);
            objectDropdown.onValueChanged.AddListener((value) => { textureProjector.ActiveChild = value; });
        } else
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
            UnityEngine.Object.Destroy(child);
        }
    }

    public void SelectNoTexture() {
        textureManager.ClearTexture();
    }

    public void OnShowTextureProperties(Component sender, object data) {
        if (!(data is TextureManager)) return;
        textureManager = (TextureManager) data;

        Clear();

        TextureProjector textureProjector = textureManager.GetComponent<TextureProjector>();
        if (textureProjector && checkChildMaterial(textureProjector)) {
            CreateUVProjection(textureProjector);
        }
        CreateTextureSelects(textureManager);
        CreateProceduralTextureSelects(textureManager);
    }

    private bool checkChildMaterial(TextureProjector textureProjector) {
        foreach (Transform child in textureProjector.transform) {
            if (child.GetComponent<MeshRenderer>().sharedMaterial.name.StartsWith("UVProjection")) {
                return true;
            }
        }

        return false;
    }
}
