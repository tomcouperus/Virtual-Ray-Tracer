Shader "Custom/VFEViewer"
{
    Properties
    {
        [Slider(3.0)]
        _WireframeWidth ("Wireframe width", Range(0., 0.2)) = 0.05
        _Color ("Color", color) = (1., 0., 0., 1.)
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent-1" "IgnoreProjector" = "True" }
        Pass {
            ZWrite Off
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }
            ColorMask 0
        }

        Pass
        {
            Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            // Stencil {
            //     Ref 1
            //     Comp NotEqual
            // }
            Blend SrcAlpha OneMinusSrcAlpha
            // ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #pragma multi_compile_fragment VERTEX EDGE FACE

            #include "UnityCG.cginc"

            struct v2g {
                float4 worldPos : SV_POSITION;
            };

            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };

            v2g vert(appdata_base v) {
                v2g o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1., 0., 0.);
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0., 0., 1.);
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0., 1., 0.);
                triStream.Append(o);
            }

            float _WireframeWidth;
            fixed4 _Color;

            fixed4 frag(g2f i) : SV_Target {
#if defined (VERTEX)
                    if(!any(bool3(i.bary.x > 1-_WireframeWidth, i.bary.y > 1-_WireframeWidth, i.bary.z > 1-_WireframeWidth)))
                        discard;
#elif defined (EDGE)
                    if(!any(bool3(i.bary.x < _WireframeWidth, i.bary.y < _WireframeWidth, i.bary.z < _WireframeWidth)))
                        discard;
#else 
                    float3 dist = abs(i.bary - float3(1./3., 1./3., 1./3.));
                    float maxDist = max(max(dist.x, dist.y), dist.z);
                    if(maxDist > _WireframeWidth)
                        discard;
#endif
                return _Color;
            }

            ENDCG
        }
    }
}
