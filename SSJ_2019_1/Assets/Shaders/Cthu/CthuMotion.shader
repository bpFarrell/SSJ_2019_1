Shader "Unlit/CthuMotion"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Normal("Normal", 2D) = "normal" {}
		_Detail("Detail", 2D) = "black" {}
    }
    SubShader
    {
Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
            CGPROGRAM
			#pragma shader_feature IS_DEPTH
			#pragma shader_feature IS_TENTS
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
				float3 normal : NORMAL;
				float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
				float3 normal : TEXCOORD3;

				half3 tspace0 : TEXCOORD4; // tangent.x, bitangent.x, normal.x
				half3 tspace1 : TEXCOORD5; // tangent.y, bitangent.y, normal.y
				half3 tspace2 : TEXCOORD6; // tangent.z, bitangent.z, normal.z
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _Normal;
			sampler2D _Detail;
			float _BossHurt;
			//float _T;
			v2f vert(appdata v,float4 vertex : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float2 uv : TEXCOORD0)
			{
                v2f o;
#if IS_TENTS
				v.vertex = CthulTent(v.vertex, v.uv, v.uv2);
#else
				v.vertex = CthulMain(v.vertex);
#endif
				o.worldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.normal = mul(UNITY_MATRIX_MV, v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);


				half3 wNormal = UnityObjectToWorldNormal(normal);
				half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
				// compute bitangent from cross product of normal and tangent
				half tangentSign = tangent.w * unity_WorldTransformParams.w;
				half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
				// output the tangent space matrix
				o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
				o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
				o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);


                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;
				
#if IS_DEPTH
			float4 clip = i.screenPos / i.screenPos.w;
			i.normal = normalize(i.normal);
			col = fixed4(i.normal.xy* 0.5 + 0.5, clip.z, 1);

			half3 tnormal = UnpackNormal(tex2D(_Normal, i.uv));

			half3 worldNormal;
			worldNormal.x = dot(i.tspace0, tnormal);
			worldNormal.y = dot(i.tspace1, tnormal);
			worldNormal.z = dot(i.tspace2, tnormal);

			col.xyz = worldNormal;
#else
			col = fixed4(.8, i.worldPos.z, .5, 1);
			col = tex2D(_MainTex, i.uv);
			col += fixed4(1,.8,.8,1)*smoothstep(0.2,0.5,pow(tex2D(_Detail, i.uv),2))*_BossHurt*10;
#endif

			return col;
            }
            ENDCG
        }
    }
}
