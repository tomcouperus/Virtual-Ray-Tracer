using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.UI.Scripts.Control_Panel;

public class ProceduralMarbleEdit : ProceduralTextureEdit {
    [SerializeField]
    private LogEdit sizeEdit;
    [SerializeField]
    private FloatEdit seedEdit;
    [SerializeField]
    private FloatEdit frequencyEdit;

    public override void SetProceduralValues(ProceduralTexture proceduralTexture) {
        ProceduralMarbleTexture marbleTexture = (ProceduralMarbleTexture)proceduralTexture;

        sizeEdit.Value = marbleTexture.Size;
        seedEdit.Value = marbleTexture.Seed;
        frequencyEdit.Value = marbleTexture.Frequency;
    }

    public override void AddListeners(ProceduralTexture proceduralTexture) {
        ProceduralMarbleTexture marbleTexture = (ProceduralMarbleTexture)proceduralTexture;

        sizeEdit.OnValueChanged.AddListener(value => marbleTexture.Size = (int)value);
        seedEdit.OnValueChanged.AddListener(value => marbleTexture.Seed = (int)value);
        frequencyEdit.OnValueChanged.AddListener(value => marbleTexture.Frequency = value);
    }
}
