Shader "Custom/VertexOffsetWithRatio"  
{  
    Properties  
    {  
        _MainTex ("Texture", 2D) = "white" {}  
        _OffsetAmount ("Offset Amount", Float) = 0.5  
        _OffsetFrequency ("Offset Frequency", Float) = 10.0  
        _OffsetStartY ("Offset Start Y (Local)", Float) = 0.0 // �޸�Ϊ����Y����  
        _OffsetEndY ("Offset End Y (Local)", Float) = 1.0    // �޸�Ϊ����Y����  
    }  
  
    SubShader  
    {  
        Tags { "RenderType"="Opaque" } // ͨ��͸�����岻�����ڻ���Y��ƫ�ƣ���������������  
        LOD 100  
  
        Pass  
        {  
            CGPROGRAM  
            #pragma vertex vert  
            #pragma fragment frag  
  
            #include "UnityCG.cginc"  
  
            struct appdata  
            {  
                float4 vertex : POSITION;  
                float2 uv : TEXCOORD0;  
            };  
  
            struct v2f  
            {  
                float2 uv : TEXCOORD0;  
                float4 vertex : SV_POSITION;  
            };  
  
            sampler2D _MainTex;  
            float4 _MainTex_ST;  
            float _OffsetAmount;  
            float _OffsetFrequency;  
            float _OffsetStartY; // ����Y����Ŀ�ʼƫ�Ƶ�  
            float _OffsetEndY;   // ����Y����Ľ���ƫ�Ƶ�  
  
            v2f vert (appdata v)  
            {  
                v2f o;  
  
                // ���������������ԭ�㣬_OffsetStartY �� _OffsetEndY ��������������ĵ�  
                float localY = v.vertex.y; // �����ģ�Ϳռ��е�Y����  
  
                // ȷ���Ƿ��ڸ�Y��Χ��Ӧ��ƫ��  
                float t = smoothstep(_OffsetStartY, _OffsetEndY, localY);  
                float offset = t * _OffsetAmount; // ƫ��������Yֵ��ָ����Χ�ڵı�����������  
  
                // Ӧ��ƫ�ƣ�������Ը�����Ҫ�������Ҳ��Ĳ�����  
                float offsetX = offset * sin(_OffsetFrequency * _Time.y + v.vertex.x * 10.0);  
                float offsetY = offset * sin(_OffsetFrequency * _Time.y + v.vertex.z * 10.0); // ע������ʹ��z����y����ΪYͨ�����ڸ߶�  
  
                // ע�⣺����ֱ���޸�v.vertex���ܻ���ĳЩ����µ������⣬��ΪUnity��shader����λ����ģ�Ϳռ���  
                // ����Ϊ����ֻ�������һ��С��ƫ�ƣ���������Ӧ���ǰ�ȫ��  
                float4 modelPosition = v.vertex;  
                modelPosition.x += offsetX;  
                modelPosition.y += offsetY;  
  
                // ��ģ�Ϳռ��λ��ת�����ü��ռ�  
                o.vertex = UnityObjectToClipPos(modelPosition);  
  
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);  
                return o;  
            }  
  
            fixed4 frag (v2f i) : SV_Target  
            {  
                fixed4 col = tex2D(_MainTex, i.uv);  
                if (col.a < 0.01) discard;  
                 
                return col;  

            }  
            ENDCG  
        }  
    }  
}