using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProceduralTexture : ScriptableObject {
    public string Name;
    public abstract Texture2D CreateTexture();
}
