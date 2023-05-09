using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFEViewer : MonoBehaviour
{
    public enum Mode
    {
        Vertex,
        Edge,
        Face
    }

    public Mode ViewMode {
        get{return viewMode;} 
        set {
            viewMode = value;
            needsUpdate = true;
        }
    }
    [SerializeField]
    private Mode viewMode = Mode.Vertex;

    
    public float WireframeWidth {
        get{return wireframeWidth;} 
        set {
            wireframeWidth = value;
            needsUpdate = true;
        }
    }
    [SerializeField, Range(0f,0.2f)]
    private float wireframeWidth;
    private Renderer[] renderers;

    private Material vfeMaterial;
    private Shader vertexView; 
    private Shader edgeView; 
    private Shader faceView; 

    private bool needsUpdate;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        vfeMaterial = Instantiate(Resources.Load<Material>(@"Materials/VFEView"));
        vertexView = Instantiate(Resources.Load<Shader>(@"Shaders/VertexView"));
        edgeView = Instantiate(Resources.Load<Shader>(@"Shaders/EdgeView"));
        faceView = Instantiate(Resources.Load<Shader>(@"Shaders/FaceView"));
        vfeMaterial.shader = vertexView;

        needsUpdate = true;
    }

    void Update() 
    {
        // if (needsUpdate) {
        //     needsUpdate = false;
        //     Debug.Log("Updating material properties");
        //     UpdateMaterialProperties();
        // }
        UpdateMaterialProperties();
    }

    void OnEnable() {
        foreach (var renderer in renderers) {
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(vfeMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    void onValidate()
    {
        needsUpdate = true;
    }
    
    void UpdateMaterialProperties()
    {
        vfeMaterial.shader = viewMode switch {
            Mode.Vertex => vertexView,
            Mode.Edge => edgeView,
            Mode.Face => faceView,
            _ => vertexView
        };
        vfeMaterial.SetFloat("_WireframeWidth", wireframeWidth);
    }
}
