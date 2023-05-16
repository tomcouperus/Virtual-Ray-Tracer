// This pass is for ambient light and is used by both transparent and opaque objects.
// Sadly this doesn't work for transparent objects if it's done via usepass.

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

#include "UnityCG.cginc"

// Light color. A built-in shader variable from "UnityCG.cginc".
uniform float4 _LightColor0;

// The shader inputs are the properties defined above.
uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform float4 _Color;
uniform float _Ambient;
uniform float _Diffuse;
uniform float _Specular;
uniform float _Shininess;

// Vertex data input to the vertex shader. For acceptable fields see:
// http://wiki.unity3d.com/index.php?title=Shader_Code.
struct vertexInput 
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
};

// Vertex shader output that is the input of the fragment shader. For acceptable fields see:
// http://wiki.unity3d.com/index.php?title=Shader_Code.

// The vertex shader.
v2f vert(vertexInput v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}

// The fragment shader.
float4 frag(v2f i) : COLOR
{
    float4 color = tex2D(_MainTex, i.uv);
    color.rgb *= _Ambient * _Color.rgb;
    color.a *= _Color.a;
    return color;
}