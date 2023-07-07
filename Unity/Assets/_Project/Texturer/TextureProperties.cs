using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _Project.UI.Scripts.Control_Panel;

/// <summary>
/// UI class that provides access to the texture properties of an object.
/// </summary>
public class TextureProperties : MonoBehaviour {

    [Header("UV mapping")]
    [SerializeField]
    private TMPro.TextMeshProUGUI UVProjectionHeader;
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
    private TMPro.TextMeshProUGUI texturesHeader;
    [SerializeField]
    private TextureSelect nullTextureSelect;
    [SerializeField]
    private TMPro.TextMeshProUGUI proceduralTexturesHeader;

    [Serializable]
    public class Event : UnityEvent { }
    [Header("Events")]
    public Event onTextureSelected;

    private TextureManager textureManager;
    private TextureProjector textureProjector;

    private List<TextureSelect> textureSelects = new List<TextureSelect>();

    public void Hide() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Creates the selectors for the textures of the given TextureManager.
    /// </summary>
    /// <param name="textureManager"></param>
    private void CreateTextureSelects(TextureManager textureManager) {
        List<Sprite> texPreviewList = textureManager.CreateTexturePreviews();
        for (int i = 0; i < texPreviewList.Count; i++) {
            int siblingIndex = proceduralTexturesHeader.transform.GetSiblingIndex();
            TextureSelect textureSelect = Instantiate(textureSelectPrefab, transform);
            textureSelect.transform.SetSiblingIndex(siblingIndex);
            
            textureSelect.SetPreview(texPreviewList[i]);
            textureSelect.SetName(texPreviewList[i].name);

            int texIndex = i;
            textureSelect.AddOnSelectListener(() => textureManager.SelectTexture(texIndex));

            textureSelects.Add(textureSelect);
        }
    }
    
    /// <summary>
    /// Creates the selectors for the procedural textures of the given TextureManager.
    /// </summary>
    /// <param name="textureManager"></param>
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
            textureSelect.AddOnSelectListener(() => {
                textureManager.SelectProceduralTexture(texIndex);
                texture.OnSelect?.Invoke();
            });

            textureSelects.Add(textureSelect);
        }
    }

    private void CreateUVProjection(TextureProjector textureProjector) {
        onTextureSelected.Invoke();

        UVProjectionHeader.gameObject.SetActive(true);
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

    /// <summary>
    /// Removes all selectors for textures and procedural textures
    /// </summary>
    private void Clear() {
        textureSelects.Clear();
        textureSelects.Add(nullTextureSelect);

        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child == texturesHeader.gameObject) continue;
            if (child == nullTextureSelect.gameObject) continue;
            if (child == proceduralTexturesHeader.gameObject) continue;
            if (child == UVProjectionHeader.gameObject) continue;
            if (child == transitionEdit.gameObject) continue;
            if (child == loopEdit.gameObject) continue;
            if (child == objectDropdown.gameObject) continue;
            UnityEngine.Object.Destroy(child);
        }
    }

    /// <summary>
    /// Action that fires when the nullTextureSelect is selected.
    /// </summary>
    public void SelectNoTexture() {
        textureManager.ClearTexture();
    }

    /// <summary>
    /// Goes over all texture select and selects the current one.
    /// Hooks them up to the event chain that is fired when a single TextureSelect
    /// is selected.
    /// </summary>
    private void AddSelectionHighlights() {
        foreach (TextureSelect texSelect in textureSelects) {
            texSelect.AddOnSelectListener(() => {
                foreach (TextureSelect t in textureSelects) {
                    if (t == texSelect) t.Selected = true;
                    else t.Selected = false;
                }
            });
        }

        textureSelects[textureManager.TextureIndex+1].Selected = true;
    }

    /// <summary>
    /// Shows the TextureProperties, as a response to a GameEvent.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
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
        AddSelectionHighlights();
    }

    private void Awake() {
        nullTextureSelect.AddOnSelectListener(() => textureManager.ClearTexture());
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
