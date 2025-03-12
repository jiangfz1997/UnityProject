Shader "Custom/SpeedLineEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpeedEffect ("Speed Intensity", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _SpeedEffect; // 控制速度线的透明度

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // ✅ 让速度线在 X 轴方向拉伸
                o.vertex.x += _SpeedEffect * 0.1;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // ✅ 让颜色变成白灰色速度线
                float speedMask = saturate(_SpeedEffect * (1 - i.uv.y)); 
                col.rgb = lerp(col.rgb, float3(1, 1, 1), speedMask); 
                
                return col;
            }
            ENDCG
        }
    }
}
