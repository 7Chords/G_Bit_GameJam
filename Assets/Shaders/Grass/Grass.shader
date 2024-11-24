Shader "Custom/VertexOffsetWithRatio"  
{  
    Properties  
    {  
        _MainTex ("Texture", 2D) = "white" {}  
        _OffsetAmount ("Offset Amount", Float) = 0.5  
        _OffsetFrequency ("Offset Frequency", Float) = 10.0  
        _OffsetStartY ("Offset Start Y (Local)", Float) = 0.0 // 修改为本地Y坐标  
        _OffsetEndY ("Offset End Y (Local)", Float) = 1.0    // 修改为本地Y坐标  
    }  
  
    SubShader  
    {  
        Tags { "RenderType"="Opaque" } // 通常透明物体不会用于基于Y的偏移，除非有特殊需求  
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
            float _OffsetStartY; // 本地Y坐标的开始偏移点  
            float _OffsetEndY;   // 本地Y坐标的结束偏移点  
  
            v2f vert (appdata v)  
            {  
                v2f o;  
  
                // 假设物体的中心在原点，_OffsetStartY 和 _OffsetEndY 是相对于物体中心的  
                float localY = v.vertex.y; // 这就是模型空间中的Y坐标  
  
                // 确定是否在该Y范围内应用偏移  
                float t = smoothstep(_OffsetStartY, _OffsetEndY, localY);  
                float offset = t * _OffsetAmount; // 偏移量根据Y值在指定范围内的比例进行缩放  
  
                // 应用偏移（这里可以根据需要调整正弦波的参数）  
                float offsetX = offset * sin(_OffsetFrequency * _Time.y + v.vertex.x * 10.0);  
                float offsetY = offset * sin(_OffsetFrequency * _Time.y + v.vertex.z * 10.0); // 注意这里使用z代替y，因为Y通常用于高度  
  
                // 注意：这里直接修改v.vertex可能会在某些情况下导致问题，因为Unity的shader期望位置在模型空间中  
                // 但因为我们只是添加了一个小的偏移，所以这里应该是安全的  
                float4 modelPosition = v.vertex;  
                modelPosition.x += offsetX;  
                modelPosition.y += offsetY;  
  
                // 将模型空间的位置转换到裁剪空间  
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