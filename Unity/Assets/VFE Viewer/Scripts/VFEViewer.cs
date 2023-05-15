using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Project.Ray_Tracer.Scripts.Utility;

/*
TODO:
- Fix udpate call with the needsUpdate or remove entirely
- Keep a list of booleans the size of vertices, edges, and faces to keep track of selected
- Make Clickable function to select vertices, edges, and faces
- Make a function to update the selected vertices, edges, and faces
    - Do this by checking with ray if the object is clicked within distance of wireframe
    - a Texture manager can invoke ray call and pass the point to Clicked() function
    Option 1:
    - Function for vertices simplest with location data
    - Edges by checking line intersection
    - Faces by checking if point is within triangle
    Option 2:
    - Create Objects to represent vertices, edges, and faces
*/

public class VFEViewer_Deprecated : MonoBehaviour
{
    public enum Mode
    {
        Vertex,
        Edge,
        Face
    }

    public struct VertexInfo
    {
        public Vector3 position;
        public Vector2 uv;
        public int index;
    }

    public Mode ViewMode {
        get{return viewMode;} 
        set {
            viewMode = value;
            // Debug.Log("ViewMode set to " + viewMode);
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
        // Debug.Log("Update called");
        // if (needsUpdate) {
        //     needsUpdate = false;
            
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
        Debug.Log("onValidate called");
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

    // Function to check where mesh is clicked.
    // If we want to do this optimized this function takes a ray and checks hit on set Mode
    // Simplest is to use Unity's raycast to check hit on object.
    private void OnMouseDown()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            return;
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;
        switch(viewMode)
        {
            case Mode.Vertex:
                getClosestVertex(mesh, hit);
                break;
            case Mode.Edge:
                Debug.Log("Edge mode not implemented");
                break;
            case Mode.Face:
                GetFace(mesh, hit);
                break;
        }
    }

    private void getClosestVertex(Mesh mesh, RaycastHit hit)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        // Debug.Log("Triangle hit at " + hit.point);
        // Debug.Log("Triangle vertices: " + p0 + ", " + p1 + ", " + p2);
        // Debug.Log("Triangle index: " + hit.triangleIndex);
        float d1 = Vector3.Distance(hit.point, p0);
        float d2 = Vector3.Distance(hit.point, p1);
        float d3 = Vector3.Distance(hit.point, p2);
        Debug.Log("Distance from hit to closest point: " + Mathf.Min(d1, d2, d3));
    }

    private void GetFace(Mesh mesh, RaycastHit hit)
    {
        // Find center of vertices of triangle
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Vector3 center = (p0 + p1 + p2) / 3;
        Debug.Log("Distance from hit to center: " + Vector3.Distance(hit.point, center));
    }
}
