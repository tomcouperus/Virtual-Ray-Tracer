using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureWrappingButton : MonoBehaviour {
    [SerializeField]
    private TextureMapper.WrapState activeState;

    public void OnWrapStateChanged(Component sender, object data) {
        TextureMapper.WrapState wrapState = (TextureMapper.WrapState)data;
        gameObject.SetActive(wrapState == activeState);
    }
}
