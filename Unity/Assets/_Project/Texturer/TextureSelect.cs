using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// UI component for selecting a texture from a TextureManager
/// </summary>
public class TextureSelect : MonoBehaviour
{
    [SerializeField]
    private Image texturePreview;
    [SerializeField]
    private TMPro.TextMeshProUGUI textureName;
    [SerializeField]
    private Toggle changeTextureMarker;
    [SerializeField]
    private Sprite indicator;

    //helper variable to ensure the toggle does not get turned off by clicking it once it's selected.
    private bool externalSelectedChange;
    private bool _selected;

    /// <summary>
    /// `true` if the TextureSelect is selected.
    /// </summary>
    /// <value></value>
    public bool Selected {
        get {return _selected;}
        set {
            if ((_selected && value) || (!_selected && !value)) return;
            externalSelectedChange = true;
            changeTextureMarker.isOn = value;
            _selected = value;
            externalSelectedChange = false;
        }
    }

    /// <summary>
    /// Sets the texturePreview to the given Sprite.
    /// </summary>
    /// <param name="preview"></param>
    public void SetPreview(Sprite preview) {
        texturePreview.sprite = preview;
    }

    /// <summary>
    /// Sets the textureName to the given string.
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name) {
        textureName.text = name;
    }

    /// <summary>
    /// Adds a UnityAction that is fired when the TextureSelect is changed to Selected.
    /// </summary>
    /// <param name="function"></param>
    public void AddOnSelectListener(UnityAction function) {
        changeTextureMarker.onValueChanged.AddListener(value => {
            if (!_selected && value) function.Invoke();
        });
    }

    private void OnDestroy() {
        if (texturePreview.sprite != indicator) {
            Object.Destroy(texturePreview.sprite);
        }
    }

    private void Awake() {
        _selected = changeTextureMarker.isOn;
        externalSelectedChange = false;
        changeTextureMarker.onValueChanged.AddListener(value => {
            if (!externalSelectedChange && !value) {
                changeTextureMarker.isOn = true;
                externalSelectedChange = false;
            }
        });
    }
}
