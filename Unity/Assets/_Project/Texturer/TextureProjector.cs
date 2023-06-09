using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer), typeof(TextureManager))]
public class TextureProjector : MonoBehaviour{

    [SerializeField]
    private Material requiredMaterial;
    public Material RequiredMaterial {get; set;} 

    private int activeChild;
    public int ActiveChild {
        get {return activeChild;}
        set {
            activeChild = SetActiveChild(value);
            Loop = Loop;
            Transition = Transition;
            if (activeChild == 1) onPyramidSelected.Invoke();
        }
    }

    [SerializeField, Range(0.00f, 1.00f)]
    private float transition = 0f;
    public float Transition {
        get {return transition;}
        set { 
            if (value == 1.00f) onFullTransition.Invoke();
            transition = value;
            transform.GetChild(ActiveChild).GetComponent<Renderer>().material.SetFloat("_Transition", transition);
        }
    }

    [SerializeField]
    private bool loop = false;
    public bool Loop {
        get {return loop;}
        set {
            loop = value;
            transform.GetChild(ActiveChild).GetComponent<Renderer>().material.SetFloat("_Loop", loop ? 1f : 0f);
        }
    }

    public int ChildCount {
        get {return transform.childCount;}
    }

    [Serializable]
    public class TextureEvent : UnityEvent { }
    public TextureEvent onFullTransition, onPyramidSelected;

    private int SetActiveChild(int index) {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == index);

        return index;
    }


    private void Awake() {
        if (transform.childCount == 0) return;
        ActiveChild = 0;
    }

    public void OnSetChildTexture(Component sender, object data) {
        if (!(data is Texture2D)) return;
        Texture2D texture = (Texture2D) data;

        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
}