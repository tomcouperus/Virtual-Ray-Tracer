using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract data object for procedural textures.
/// </summary>
public abstract class ProceduralTexture : ScriptableObject {
    public string Name;
    
    [SerializeField]
    private ProceduralTextureEdit _proceduralTextureEditPrefab;
    /// <summary>
    /// The UI component prefab that interacts with the ProceduralTexture
    /// </summary>
    /// <value></value>
    public ProceduralTextureEdit ProceduralTextureEditPrefab {
        get {return _proceduralTextureEditPrefab;}
    }

    /// <summary>
    /// Event that fires on selection of the texture
    /// </summary>
    public UnityEvent OnSelect;
    
    /// <summary>
    /// Action for refreshing the texture. 
    /// Used in ProceduralTextureEdit.AddListeners
    /// </summary>
    public UnityAction RefreshTextureAction;

    /// <summary>
    /// Creates a texture for the settings of the ProceduralTexture
    /// </summary>
    /// <returns></returns>
    public abstract Texture2D CreateTexture();

    /// <summary>
    /// Creates a smaller version of CreateTexture
    /// </summary>
    /// <returns></returns>
    public abstract Texture2D CreatePreviewTexture();
}
