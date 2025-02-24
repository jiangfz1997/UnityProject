Shader "Custom/HurtEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HurtColor ("Hurt Color", Color) = (0.8, 0, 0, 1) 
        _HurtIntensity ("Hurt Intensity", Range(0, 1)) = 0 
        _FlashControl ("Flash Control", Range(0, 1)) = 0 // 控制透明度变化
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off 
        Lighting Off 
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _HurtColor;
            float _HurtIntensity;
            float _FlashControl; // 透明度控制

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 颜色渐变（受伤时变红）
                col.rgb = lerp(col.rgb, _HurtColor.rgb, _HurtIntensity); 

                // 透明度控制（0.5 ~ 1 之间变化）
                col.a *= lerp(0.5, 1.0, _FlashControl); 

                return col;
            }
            ENDCG
        }
    }
}
