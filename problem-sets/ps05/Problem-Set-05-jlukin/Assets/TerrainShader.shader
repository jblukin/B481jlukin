Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Ambient Color", COLOR) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {

            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct VertexIn
            {

                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv0 : TEXCOORD0;
           
            };

            struct VertexOut
            {

                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float2 uv0 : TEXCOORD0;
            
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _MainColor;
            float3 _LightPos;
            float _Smoothness;

            VertexOut vert ( VertexIn v )
            {

                VertexOut o;

                o.uv0 = v.uv0;
                
                o.normal = UnityObjectToWorldNormal( v.normal );

                o.worldPos = mul( unity_ObjectToWorld, v.vertex );

                float y_noise = ( 1 / ( v.vertex.x * v.vertex.z ) + 0.5 ) + cos( 0.25 + v.vertex.x * v.vertex.z ) * sin( 0.75 + exp( v.vertex.x ) / exp( v.vertex.z ) );

                float4 noiseVertex = float4( v.vertex.x, y_noise, v.vertex.z, v.vertex.w) ;

                o.vertex = UnityObjectToClipPos( noiseVertex );

                return o;

            }

            fixed4 frag ( VertexOut o ) : SV_Target
            {

                float3 vertex = o.vertex.xyz;
                float3 normal = normalize( o.normal );
                
                float3 lightColor = _LightColor0.rgb;
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                
                float falloff = DotClamped( normalize( lightDir ), normal );
                float3 diffuse = lightColor * falloff;

                float3 viewDir = normalize( _WorldSpaceCameraPos - o.worldPos );
                float3 reflectionDir = reflect( -lightDir, o.normal );
                float3 specular = lightColor * pow( DotClamped( viewDir, reflectionDir ), _Smoothness * 100 );

                float3 ambient = _MainColor.rgb;

                return float4( ambient + diffuse + specular, 0 );

            }
            ENDCG
        }
    }
}
