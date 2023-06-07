using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TextureMapper : MonoBehaviour {

    public enum WrapState {Wrapped, Unwrapped, Transforming}
    [SerializeField]
    private WrapState wrapState;

    [SerializeField]
    private MeshRenderer fullTextureRenderer;
    public bool ShowFullTexture {
        get { return fullTextureRenderer.gameObject.activeSelf; }
        set {
            fullTextureRenderer.gameObject.SetActive(value);
        }
    }

    [Header("Animation")]
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    [Min(0.1f)]
    private float animationTime = 3f;
    [SerializeField]
    [Range(0,1)]
    private float animationProgress;

    private Mesh wrappedMesh;
    private Mesh unwrappedMesh;
    private Mesh lerpedMesh;

    [SerializeField]
    private MeshFilter targetMeshFilter;
    [SerializeField]
    private MeshRenderer targetMeshRenderer;

    [Header("Events")]
    [SerializeField]
    private GameEvent onWrapStateChanged;

    private Mesh CopyMesh(Mesh mesh, string name = null) {
        Mesh copy = new Mesh();
        if (name == null) copy.name = mesh.name+"(Clone)";
        else copy.name = name;
        copy.vertices = mesh.vertices;
        copy.triangles = mesh.triangles;
        copy.uv = mesh.uv;
        copy.normals = mesh.normals;
        return copy;
    }

    private Mesh UnwrapMesh(Mesh mesh) {
        Mesh unwrapped = CopyMesh(mesh, mesh.name+" (Unwrapped)");
        Vector3[] vertices = unwrapped.vertices;
        for (int i = 0; i < unwrapped.vertexCount; i++) {
            Vector2 uv = unwrapped.uv[i] * 2 - Vector2.one;
            vertices[i] = new Vector3(uv.x, uv.y, 0);
        }
        unwrapped.vertices = vertices;
        unwrapped.RecalculateNormals();
        return unwrapped;
    }

    private void LerpMesh(float t) {
        Vector3[] vertices = lerpedMesh.vertices;
        for (int i = 0; i < lerpedMesh.vertexCount; i++) {
            Vector3 wrappedVertex = wrappedMesh.vertices[i];
            Vector3 unwrappedVertex = unwrappedMesh.vertices[i];
            Vector3 vertex = Vector3.Lerp(wrappedVertex, unwrappedVertex, t);
            vertices[i] = vertex;
        }
        lerpedMesh.vertices = vertices;
        lerpedMesh.RecalculateNormals();
    }

    public void Unwrap() {
        StartCoroutine(UnwrapAnimation());
    }

    private IEnumerator UnwrapAnimation() {
        onWrapStateChanged.Raise(this, WrapState.Transforming);
        targetMeshFilter.gameObject.SetActive(true);
        float deltaTime = 0;
        while (deltaTime < animationTime) {
            animationProgress = deltaTime / animationTime;
            
            LerpMesh(animationProgress);
            targetMeshFilter.transform.localPosition = targetPosition * animationProgress;

            deltaTime += Time.deltaTime;
            yield return null;
        }
        animationProgress = 1;
        onWrapStateChanged.Raise(this, WrapState.Unwrapped);
    }

    public void Wrap() {
        StartCoroutine(WrapAnimation());
    }

    private IEnumerator WrapAnimation() {
        if (ShowFullTexture) {
            ShowFullTexture = false;
            yield return new WaitForSeconds(0.25f);
        }
        onWrapStateChanged.Raise(this, WrapState.Transforming);
        float deltaTime = 0;
        while (deltaTime < animationTime) {
            animationProgress = 1 - deltaTime / animationTime;

            LerpMesh(animationProgress);
            targetMeshFilter.transform.localPosition = targetPosition * animationProgress;

            deltaTime += Time.deltaTime;
            yield return null;
        }
        animationProgress = 0;
        targetMeshFilter.gameObject.SetActive(false);
        onWrapStateChanged.Raise(this, WrapState.Wrapped);
    }

    void Awake() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        wrappedMesh = meshFilter.mesh;
        unwrappedMesh = UnwrapMesh(wrappedMesh);
        lerpedMesh = CopyMesh(meshFilter.mesh, meshFilter.mesh+" (Lerped)");
        targetMeshFilter.mesh = lerpedMesh;
        wrapState = WrapState.Wrapped;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        targetMeshRenderer.material = meshRenderer.material;

        fullTextureRenderer.transform.position = targetPosition;
        fullTextureRenderer.material = meshRenderer.material;
        ShowFullTexture = false;
    }

    public void Select() {
        onWrapStateChanged.Raise(this, wrapState);
    }

    public void SetWrapState(Component sender, object data) {
        if (sender != this) return;
        wrapState = (WrapState) data;
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        if (!enabled) return;
        if (!Application.isPlaying) return;
        if (lerpedMesh) LerpMesh(animationProgress);
        targetMeshFilter.transform.localPosition = targetPosition * animationProgress;
    }
    #endif
}
