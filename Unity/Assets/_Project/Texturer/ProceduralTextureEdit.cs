using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralTextureEdit : MonoBehaviour {
    public abstract void SetProceduralValues(ProceduralTexture proceduralTexture);
    public abstract void AddListeners(ProceduralTexture proceduralTexture, TextureSelect texSelect);
}
