using System.Collections;
using System.Collections.Generic;
using _Project.UI.Scripts.Control_Panel;
using UnityEngine;

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

    public override void AddListeners(ProceduralTexture proceduralTexture) {
        ProceduralBrickTexture brickTexture = (ProceduralBrickTexture)proceduralTexture;

        brickColorEdit.OnValueChanged.AddListener(value => brickTexture.BrickColor = value);
        mortarColorEdit.OnValueChanged.AddListener(value => brickTexture.MortarColor = value);
        brickWidthEdit.OnValueChanged.AddListener(value => brickTexture.BrickWidth = (int)value);
        brickHeightEdit.OnValueChanged.AddListener(value => brickTexture.BrickHeight = (int)value);
        bricksXEdit.OnValueChanged.AddListener(value => brickTexture.BricksX = (int)value);
        bricksYEdit.OnValueChanged.AddListener(value => brickTexture.BricksY = (int)value);
        brickOffsetEdit.OnValueChanged.AddListener(value => brickTexture.BrickOffset = value);
        mortarThicknessEdit.OnValueChanged.AddListener(value => brickTexture.MortarThickness = (int)value);
    }
}
