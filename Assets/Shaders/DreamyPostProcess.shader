Shader "Custom/DreamyPostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Range(0, 1)) = 0.5
        _BlurSize ("Blur Size", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Used for blur calculations
            float _Distortion;
            float _BlurSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Distortion effect
                float2 uvDistorted = i.uv + float2(sin(i.uv.y * 10.0) * _Distortion, cos(i.uv.x * 10.0) * _Distortion);
                col += tex2D(_MainTex, uvDistorted) * 0.5; // Blend distorted texture

                // Simple blur effect (box blur)
                fixed4 blurCol = fixed4(0,0,0,0);
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        blurCol += tex2D(_MainTex, i.uv + float2(x, y) * _BlurSize * _MainTex_TexelSize.xy);
                    }
                }
                col = lerp(col, blurCol / 9.0, 0.5); // Mix original and blurred color

                return col;
            }
            ENDCG
        }
    }
}
