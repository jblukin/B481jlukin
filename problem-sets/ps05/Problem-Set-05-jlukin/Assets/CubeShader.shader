Shader "Unlit/CubeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                nointerpolation float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4x4 _ModelMatrix;
            float4x4 _ViewingMatrix;

            v2f vert (appdata v)
            {
                v2f o;
                
                
                o.vertex = mul(UNITY_MATRIX_P, mul(_ViewingMatrix, mul(_ModelMatrix, v.vertex)));

                //o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                col = float4(i.color.rgb, 1.0);

                return col;
            }
            ENDCG
        }
    }
}
