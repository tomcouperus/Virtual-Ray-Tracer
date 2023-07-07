using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component that displays the current texture of an object and has a button
/// to open the TextureProperties
/// 
/// </summary>
public class TextureEdit : MonoBehaviour {
    
    [Header("UI")]
    [SerializeField]
    private Image texturePreview;
    [SerializeField]
    private TMPro.TextMeshProUGUI noTextureLabel;
    [SerializeField]
    private Button changeTextureButton;

    private TextureManager currentManager;

    [Header("Events")]
    [SerializeField]
    private GameEvent onShowTextureProperties;

    private void UpdateTexturePreview(Sprite preview) {
        if (preview) {
            if (texturePreview.sprite != null) {
                Object.Destroy(texturePreview.sprite);
            }
            texturePreview.sprite = preview;
            texturePreview.gameObject.SetActive(true);
            noTextureLabel.gameObject.SetActive(false);
        } else {
            texturePreview.gameObject.SetActive(false);
            noTextureLabel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the current texture manager and updates the texture preview
    /// </summary>
    /// <param name="textureManager"></param>
    public void SetCurrentTextureManager(TextureManager textureManager) {
        currentManager = textureManager;
        UpdateTexturePreview(currentManager.CreateTexturePreview());
    }

    private void Awake() {
        changeTextureButton.onClick.AddListener(() => {onShowTextureProperties.Raise(this, currentManager);});
    }

}
