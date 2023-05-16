using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TextureSampler : MonoBehaviour
{
    [SerializeField]
    private Texture2D texture;

    [SerializeField]
    private FilterMode _filterMode;
    public FilterMode FilterMode {
        get {return _filterMode;}
        set {
            _filterMode = value;
            if (texture) texture.filterMode = _filterMode;
        }
    }

    private void Awake() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        texture = meshRenderer.material.mainTexture as Texture2D;
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        FilterMode = _filterMode;
    }
    #endif
}
