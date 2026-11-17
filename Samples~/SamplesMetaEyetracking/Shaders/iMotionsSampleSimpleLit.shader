// Simple lit shader for the iMotions sample scene.
// It has no render pipeline specific pass, so it renders in the Built-in Render Pipeline, URP and HDRP
// (the Built-in "Standard" shader shows pink in URP and HDRP). Supports single pass instanced stereo for VR.
Shader "iMotions/Sample Simple Lit"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _UseVertexColor ("Use Vertex Color (Line Renderers)", Float) = 0
        [Toggle] _Lit ("Simple Lighting", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _UseVertexColor;
            float _Lit;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.color = lerp(fixed4(1, 1, 1, 1), v.color, _UseVertexColor);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                // Fixed light from above, so the scene doesn't depend on the pipeline's lights.
                float3 n = i.normal / max(length(i.normal), 1e-4);
                float diffuse = saturate(dot(n, normalize(float3(0.4, 1.0, 0.3))));
                float light = lerp(1.0, 0.55 + 0.45 * diffuse, _Lit);
                fixed4 c = tex2D(_MainTex, i.uv) * _Color * i.color;
                c.rgb *= light;
                return c;
            }
            ENDCG
        }
    }
    Fallback Off
}
