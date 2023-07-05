using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an edge of a mesh.
/// </summary>
public class Edge : MonoBehaviour {

    public float Radius = 0.015f;

    private Vector3 _direction;
    /// <summary>
    /// Controls the direction of the edge line
    /// </summary>
    public Vector3 Direction {
        get {return _direction;}
        set {
            _direction = value;
            transform.up = _direction;
        }
    }

    private Vector3 _position;
    /// <summary>
    /// Controls the position of the middle of the edge line
    /// </summary>
    public Vector3 Position {
        get {return _position;}
        set {
            _position = value;
            transform.localPosition = _position;
        }
    }

    private float _length;
    /// <summary>
    /// Controls the length of the edge line
    /// </summary>
    public float Length {
        get {return _length;}
        set {
            _length = value;
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(Radius, 0.5f * _length, Radius);
            // Since the cylinder mesh is twice as high as it's wide, multiply the length by half for scale.
        }
    }
}
