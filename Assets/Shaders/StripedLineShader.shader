Shader "Custom/StripedLineShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1) // Red
        _Color2 ("Color 2", Color) = (0, 0, 1, 1) // Blue
        _StripeWidth ("Stripe Width", Float) = 0.1
        _Tiling ("Tiling", Float) = 10.0
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

            float4 _Color1;
            float4 _Color2;
            float _StripeWidth;
            float _Tiling;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Pass through the original UVs but adjust the x coordinate for tiling
                o.uv = v.uv;
                o.uv.x *= _Tiling;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate the striped pattern
                float modResult = fmod(i.uv.x, 1.0);
                bool inStripe = modResult < _StripeWidth;
                return inStripe ? _Color1 : _Color2;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
