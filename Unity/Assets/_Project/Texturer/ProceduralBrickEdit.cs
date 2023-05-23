using System.Collections;
using System.Collections.Generic;
using _Project.UI.Scripts.Control_Panel;
using UnityEngine;

public class ProceduralBrickEdit : ProceduralTextureEdit {

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

        brickWidthEdit.Value = brickTexture.BrickWidth;
        brickHeightEdit.Value = brickTexture.BrickHeight;
        bricksXEdit.Value = brickTexture.BricksX;
        bricksYEdit.Value = brickTexture.BricksY;
        brickOffsetEdit.Value = brickTexture.BrickOffset;
        mortarThicknessEdit.Value = brickTexture.MortarThickness;
    }

    public override void AddListeners(ProceduralTexture proceduralTexture) {
        ProceduralBrickTexture brickTexture = (ProceduralBrickTexture)proceduralTexture;

        brickWidthEdit.OnValueChanged.AddListener(value => {brickTexture.BrickWidth = (int)value;});
        brickHeightEdit.OnValueChanged.AddListener(value => {brickTexture.BrickHeight = (int)value;});
        bricksXEdit.OnValueChanged.AddListener(value => {brickTexture.BricksX = (int)value;});
        bricksYEdit.OnValueChanged.AddListener(value => {brickTexture.BricksY = (int)value;});
        brickOffsetEdit.OnValueChanged.AddListener(value => {brickTexture.BrickOffset = (int)value;});
        mortarThicknessEdit.OnValueChanged.AddListener(value => {brickTexture.MortarThickness = (int)value;});
    }
}
