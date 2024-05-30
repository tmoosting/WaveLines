Shader "Custom/MovingPatternShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1.0
        _YScale ("Y Scale", Float) = 1.0
        _Speed ("Speed", Float) = 1.0 // Property to control the speed of texture movement
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale;
            float _YScale;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Convert vertex position to world space
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate the texture offset based on time and speed
                float textureOffset = _Time.y * _Speed;

                // Apply the texture offset along with scaling
                o.uv.x = (worldPos.x + textureOffset) * _Scale;
                o.uv.y = worldPos.y * _YScale;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Wrap the texture coordinates to ensure seamless tiling
                float2 uvWrapped = float2(fmod(i.uv.x, 1.0), fmod(i.uv.y, 1.0));
                return tex2D(_MainTex, uvWrapped);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
