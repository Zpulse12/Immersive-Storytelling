Shader "Custom/rainbow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 uv : TEXCOORD0;
            };

            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _Speed;
                float3 color = float3(
                    sin(t + i.uv.x) * 0.5 + 0.5,
                    sin(t + i.uv.y + 2.0) * 0.5 + 0.5,
                    sin(t + i.uv.z + 4.0) * 0.5 + 0.5
                );
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
