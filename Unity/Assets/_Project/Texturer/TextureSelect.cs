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
    private Button changeTextureButton;
    [SerializeField]
    private Sprite indicator;

    public bool Selected {
        get {return textureName.fontStyle == TMPro.FontStyles.Bold;}
        set {
            if (value) textureName.fontStyle = TMPro.FontStyles.Bold;
            else textureName.fontStyle = TMPro.FontStyles.Normal;
        }
    }

    public void SetPreview(Sprite preview) {
        texturePreview.sprite = preview;
    }

    public void SetName(string name) {
        textureName.text = name;
    }

    public void AddOnClickListener(UnityAction function) {
        changeTextureButton.onClick.AddListener(function);
    }

    private void OnDestroy() {
        if (texturePreview.sprite != indicator) {
            Object.Destroy(texturePreview.sprite);
        }
    }
}
