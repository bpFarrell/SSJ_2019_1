Shader "Unlit/CthuMotion 1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
            CGPROGRAM
			#pragma shader_feature IS_DEPTH
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Assets/Cthul.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float2 uv2 :TEXCOORD1;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
				float3 normal : TEXCOORD3;
				
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			//float _T;
            v2f vert (appdata v)
            {
                v2f o;
				v.vertex = CthulTent(v.vertex,v.uv,v.uv2);
				o.worldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.normal = mul(UNITY_MATRIX_MV, v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;
#if IS_DEPTH
			float4 clip = i.screenPos / i.screenPos.w;
			i.normal = normalize(i.normal);
			col = fixed4(i.normal.xy* 0.5 + 0.5, clip.z, 1);
#else
			col = fixed4(.8, i.worldPos.z, .5, 1);
#endif

			return col;
            }
            ENDCG
        }
    }
}
