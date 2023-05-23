using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralTexture : ScriptableObject {
    public string Name;
    
    [SerializeField]
    private ProceduralTextureEdit _proceduralTextureEditPrefab;
    public ProceduralTextureEdit ProceduralTextureEditPrefab {
        get {return _proceduralTextureEditPrefab;}
    }

    public abstract Texture2D CreateTexture();
}
