Shader "Custom/RingWarning"
{
    Properties
    {
        _Color ("Color", Color) = (1,0,0,1)
        _InnerRadius ("Inner Radius", Range(0,1)) = 0.6
        _FadeWidth ("Fade Width", Range(0,0.5)) = 0.1
        _Opacity ("Opacity", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            float4 _Color;
            float _InnerRadius;
            float _FadeWidth;
            float _Opacity;

            v2f vert(appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o; }

            fixed4 frag(v2f i) : SV_Target
            {
                // Convert UVs to distance from center (0 to 1)
                float2 centered = i.uv - 0.5;
                float dist = length(centered) * 2;

                // Fade in at inner edge, fade out at outer edge
                float inner = smoothstep(_InnerRadius, _InnerRadius + _FadeWidth, dist);
                float outer = smoothstep(1.0, 1.0 - _FadeWidth, dist);
                float alpha = inner * outer * _Opacity;

                return fixed4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}