using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural Texture/Marble")]
public class ProceduralMarbleTexture : ProceduralTexture {

    [SerializeField]
    private int size = 512;
    [SerializeField]
    private int seed;
    [SerializeField]
    private float frequency = 3f;

    [SerializeField]
    private RandomNoise roughNoise;
    [SerializeField]
    private RandomNoise detailNoise;

    [SerializeField]
    [Range(0,1)]
    private float detailBlend;

    public override Texture2D CreateTexture() {
        Texture2D tex = new Texture2D(size, size);
        tex.name = Name;
        
        Color[] colorMap = new Color[size * size];

        System.Random prng = new System.Random(seed);

        roughNoise.Init(prng.Next());
        float[,] roughNoiseMap = roughNoise.PerlinMap(size, size);
        
        detailNoise.Init(prng.Next());
        float[,] detailNoiseMap = detailNoise.PerlinMap(size, size);
        
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float t = (float)x/size;
                t += roughNoiseMap[x,y] * 2 - 1;
                float phase = 0.5f;
                float sample = -Mathf.Cos(2*Mathf.PI * (frequency * t + phase) + Mathf.Sin(2*Mathf.PI * (frequency * t + phase))); // [-1, 1]
                sample = (sample + 1) / 2f; // [0, 1]

                Color roughColor = Color.Lerp(Color.black, Color.white, sample);
                Color detailColor = Color.Lerp(Color.black, Color.white, detailNoiseMap[x,y]);
                
                colorMap[x + y*size] = Color.Lerp(roughColor, detailColor, detailBlend);
            }
        }

        tex.SetPixels(colorMap);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        tex.Apply();
        return tex;
    }
}

[System.Serializable]
class RandomNoise {
    [SerializeField]
    [Min(1)]
    private int octaves = 8;
    [SerializeField]
    [Min(0.0001f)]
    private float scale = 1;
    [SerializeField]
    private float persistence = 0.5f;
    [SerializeField]
    private float lacunarity = 2f;
    Vector2[] octaveOffsets;

    public void Init(int seed) {
        octaveOffsets = new Vector2[octaves];

        System.Random prng = new System.Random(seed);
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
    }

    public float Perlin(float x, float y) {
        float value = 0;
        float amplitude = 1;
        float frequency = 1;
        for (int i = 0; i < octaveOffsets.Length; i++) {

            float sampleX = x / scale * frequency + octaveOffsets[i].x;
            float sampleY = y / scale * frequency + octaveOffsets[i].y;
            float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
            value += perlin * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return value;
    }

    ///<summary>
    /// Returns a 2-dimensional map with values between 0.0 and 1.0
    ///</summary>
    public float[,] PerlinMap(int width, int height) {
        float[,] noiseMap = new float[width, height];
        float minNoise = float.MaxValue;
        float maxNoise = float.MinValue;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float value = Perlin(x, y);
                if (value < minNoise) minNoise = value;
                else if (value > maxNoise) maxNoise = value;
                noiseMap[x,y] = value;
            }
        }
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x,y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x,y]);
            }
        }
        return noiseMap;
    }

}
