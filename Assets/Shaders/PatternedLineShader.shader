Shader "Custom/PatternedLineShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0.3, 0.5, 0.8, 1)
        _BorderColor ("Border Color", Color) = (0.2, 0.3, 0.5, 1)
        _Scale ("Scale", Float) = 10.0
        _BorderSize ("Border Size", Float) = 0.05
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

            float4 _MainColor;
            float4 _BorderColor;
            float _Scale;
            float _BorderSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Scale;
                return o;
            }

            // Sample a procedural Voronoi pattern
        float2 random2(float2 p) { return frac(sin(float2(dot(p, float2(127.1,311.7)), dot(p, float2(269.5,183.3))))*43758.5453); }

float voronoi(float2 x)
{
    float2 n = floor(x);
    float2 f = frac(x);
    float id = 0.0;
    float2 res = float2(8.0, 8.0); // Correction here, needs two components

    for (int j = -1; j <= 1; j++)
    {
        for (int i = -1; i <= 1; i++)
        {
            float2 b = float2(i, j);
            float2 r = float2(b) - f + random2(n + b);
            float d = dot(r, r);

            if (d < res.x)
            {
                id = dot(n + b, float2(1.0, 1.0));
                res.y = res.x;
                res.x = d;
            }
        }
    }
    return res.y - res.x; // Returns the difference to create the edges
}

            fixed4 frag (v2f i) : SV_Target
            {
                float d = voronoi(i.uv);
                float edge = smoothstep(_BorderSize, _BorderSize + 0.02, d);
                return lerp(_BorderColor, _MainColor, edge);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
