using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract UI component for ProceduralTextures
/// </summary>
public abstract class ProceduralTextureEdit : MonoBehaviour {
    /// <summary>
    /// Sets the values of the UI to those of the ProceduralTexture
    /// </summary>
    /// <param name="proceduralTexture"></param>
    public abstract void SetProceduralValues(ProceduralTexture proceduralTexture);
    
    /// <summary>
    /// Adds listeners to the UI components of this object.
    /// Makes the UI affect the values of the given ProceduralTexture
    /// </summary>
    /// <param name="proceduralTexture"></param>
    /// <param name="texSelect"></param>
    public abstract void AddListeners(ProceduralTexture proceduralTexture, TextureSelect texSelect);
}
