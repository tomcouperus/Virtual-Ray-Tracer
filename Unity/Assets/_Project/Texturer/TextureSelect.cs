using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public void SetPreview(Sprite preview) {
        texturePreview.sprite = preview;
    }

    public void SetName(string name) {
        textureName.text = name;
    }

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
