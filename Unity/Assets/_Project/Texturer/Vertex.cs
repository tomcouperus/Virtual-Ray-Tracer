using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour {

    public void SetPosition(Vector3 position) {
        transform.position = position + transform.parent.position;
    }
}
