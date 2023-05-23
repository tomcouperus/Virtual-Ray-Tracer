using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural Texture/Brick")]
public class ProceduralBrickTexture : ProceduralTexture {

    [Range(1, 200)]
    public int BrickWidth = 19;
    [Range(1, 100)]
    public int BrickHeight = 9;
    [Range(1, 10)]
    public int BricksX = 4;
    [Range(1,10)]
    public int BricksY = 10;
    [Range(0,1)]
    public float BrickOffset = 0.5f;
    [Range(1, 10)]
    public int MortarThickness = 1;

    public override Texture2D CreateTexture() {
        int width = BrickWidth * BricksX + MortarThickness * (BricksX-1);
        int height = BrickHeight * BricksY + MortarThickness * (BricksY-1);

        Texture2D tex = new Texture2D(width, height);
        tex.name = Name;

        Color[] colorMap = new Color[width * height];

        bool isBrickY = true;
        int brickY = 0;
        int mortarY = 0;
        for (int y = 0; y < height; y++) {
            bool isBrickX = true;
            int row = brickY / BrickHeight;
            int brickX = Mathf.RoundToInt(row * BrickOffset * BrickWidth);
            int mortarX = 0;
            for (int x = 0; x < width; x++) {
                Color color = Color.black;
                if (isBrickX && isBrickY) {
                    color = Color.red;
                    brickX++;
                    if (brickX % BrickWidth == 0) isBrickX = false;
                } else {
                    color = Color.gray;
                    mortarX++;
                    if (mortarX % MortarThickness == 0) isBrickX = true;
                }
                colorMap[x + y*width] = color;
            }
            if (isBrickY) {
                brickY++;
                if (brickY % BrickHeight == 0) isBrickY = false;
            } else {
                mortarY++;
                if (mortarY % MortarThickness == 0) isBrickY = true;
            }
        }
        tex.SetPixels(colorMap);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return tex;
    }
}
