using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public float Radius = 0.015f;

    public void SetFromTo(Vector3 from, Vector3 to) {
        Vector3 Direction = to-from;
        transform.localPosition = from + (Direction)/2;
        transform.up = Direction;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, scale.y * Direction.magnitude, scale.z);
    }

    private Vector3 _direction;
    public Vector3 Direction {
        get {return _direction;}
        set {
            _direction = value;
            transform.up = _direction;
        }
    }

    private Vector3 _position;
    public Vector3 Position {
        get {return _position;}
        set {
            _position = value;
            transform.localPosition = _position;
        }
    }

    private float _length;
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
