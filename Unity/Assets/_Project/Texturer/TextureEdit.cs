using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureEdit : MonoBehaviour {
    
    [SerializeField]
    private Image texturePreview;
    [SerializeField]
    private TMPro.TextMeshProUGUI noTextureLabel;

    public void UpdateTexturePreview(Sprite preview) {
        if (preview) {
            texturePreview.sprite = preview;
            texturePreview.gameObject.SetActive(true);
            noTextureLabel.gameObject.SetActive(false);
        } else {
            texturePreview.gameObject.SetActive(false);
            noTextureLabel.gameObject.SetActive(true);
        }
    }

}
