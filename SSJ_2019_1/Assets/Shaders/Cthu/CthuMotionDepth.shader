Shader "Unlit/CthuMotionDepth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

			//float _T;
            #include "UnityCG.cginc"
			#include "Assets/Cthul.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            v2f vert (appdata v)
            {
                v2f o;
				v.vertex = CthulMain(v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.normal = mul(UNITY_MATRIX_MV, v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

				float4 clip = i.screenPos / i.screenPos.w;
                return fixed4(i.normal.xy*0.5+0.5,clip.z,1);
            }
            ENDCG
        }
    }
}
