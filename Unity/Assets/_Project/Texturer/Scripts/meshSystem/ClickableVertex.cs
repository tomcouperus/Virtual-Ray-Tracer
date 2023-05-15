using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableVertex : MonoBehaviour {

    public int vertexIndex;
    public Vector3 vertex;
    public Vector2 uv;
    private void OnMouseDown() {
        Debug.Log(name+": "+vertex+"; uv: "+uv);
    }

    public void SetVertex(int vertexIndex, Vector3 vertex, Vector2 uv) {
        this.vertexIndex = vertexIndex;
        this.vertex = vertex;
        this.uv = uv;
    }
}
