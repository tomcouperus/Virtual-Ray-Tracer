// Phong shader to match the ray tracer. Resources used:
// https://janhalozan.com/2017/08/12/phong-shader/
// http://viclw17.github.io/2016/04/19/phong-shading-model-walkthrough/
// Shadow support based on:
// https://docs.unity3d.com/560/Documentation/Manual/SL-VertexFragmentShaderExamples.html
// Note that #pragma multi_compile_fwdadd_fullshadows is required in the add pass for point light shadows.


Shader "Custom/RayTracerShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Ambient("Ambient", Range(0, 1)) = 0.2
        _Diffuse("Diffuse", Range(0,1)) = 1.0
        _Specular("Specular", Range(0, 1)) = 0.5
        _Shininess("Shininess", Float) = 32
        _RefractiveIndex("RefractiveIndex", Range(1,1)) = 1
    }

    SubShader
    {
        Tags {"RenderType" = "Opaque" }
        LOD 200
        
        // We render both the inside and the outside of objects. This way you can enter them if you want.
        Cull off
        
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #include "GeneralShader.cginc"
            ENDCG
        }

        UsePass "Custom/LightShader/LIGHTPASS"
    }

    FallBack "VertexLit" // Any fall back shader that supports casting shadows will do.
}
