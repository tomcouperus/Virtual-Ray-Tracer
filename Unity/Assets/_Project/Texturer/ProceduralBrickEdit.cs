using System.Collections;
using System.Collections.Generic;
using _Project.UI.Scripts.Control_Panel;
using UnityEngine;

/// <summary>
/// UI component for the ProceduralBrickTexture
/// </summary>
public class ProceduralBrickEdit : ProceduralTextureEdit {

    [SerializeField]
    private ColorEdit brickColorEdit;
    [SerializeField]
    private ColorEdit mortarColorEdit;
    [SerializeField]
    private FloatEdit brickWidthEdit;
    [SerializeField]
    private FloatEdit brickHeightEdit;
    [SerializeField]
    private FloatEdit bricksXEdit;
    [SerializeField]
    private FloatEdit bricksYEdit;
    [SerializeField]
    private FloatEdit brickOffsetEdit;
    [SerializeField]
    private FloatEdit mortarThicknessEdit;

    public override void SetProceduralValues(ProceduralTexture proceduralTexture) {
        ProceduralBrickTexture brickTexture = (ProceduralBrickTexture)proceduralTexture;

        brickColorEdit.Color = brickTexture.BrickColor;
        mortarColorEdit.Color = brickTexture.MortarColor;
        brickWidthEdit.Value = brickTexture.BrickWidth;
        brickHeightEdit.Value = brickTexture.BrickHeight;
        bricksXEdit.Value = brickTexture.BricksX;
        bricksYEdit.Value = brickTexture.BricksY;
        brickOffsetEdit.Value = brickTexture.BrickOffset;
        mortarThicknessEdit.Value = brickTexture.MortarThickness;
    }

    public override void AddListeners(ProceduralTexture proceduralTexture, TextureSelect texSelect) {
        ProceduralBrickTexture brickTexture = (ProceduralBrickTexture)proceduralTexture;

        brickColorEdit.OnValueChanged.AddListener(value => {
            brickTexture.BrickColor = value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        mortarColorEdit.OnValueChanged.AddListener(value => {
            brickTexture.MortarColor = value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        brickWidthEdit.OnValueChanged.AddListener(value => {
            brickTexture.BrickWidth = (int)value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        brickHeightEdit.OnValueChanged.AddListener(value => {
            brickTexture.BrickHeight = (int)value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        bricksXEdit.OnValueChanged.AddListener(value => {
            brickTexture.BricksX = (int)value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        bricksYEdit.OnValueChanged.AddListener(value => {
            brickTexture.BricksY = (int)value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        brickOffsetEdit.OnValueChanged.AddListener(value => {
            brickTexture.BrickOffset = value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
        mortarThicknessEdit.OnValueChanged.AddListener(value => {
            brickTexture.MortarThickness = (int)value;
            if (texSelect.Selected) brickTexture.RefreshTextureAction.Invoke();
        });
    }
}
