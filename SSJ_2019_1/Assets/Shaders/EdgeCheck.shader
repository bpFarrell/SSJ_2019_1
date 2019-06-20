Shader "Hidden/EdgeCheck"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DepthSensitivity("Depth Sensitivity",Float) = 0.0004
		_NormalSensitivity("Normal Sensitivity",Float) = .93
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				
		#include "UnityCG.cginc"
		#include "Assets/ShaderBox.cginc"

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;
			half4 _CameraDepthNormalsTexture_ST;
			float _DepthSensitivity;
			float _NormalSensitivity;
			float effect_GetGBufferDifference(fixed4 a, fixed4 b) {
				float deltaDepth = abs(DecodeFloatRG(a.zw) - DecodeFloatRG(b.zw)) > _DepthSensitivity;
				float2 diffNormal = abs(a.xy - b.xy)* _NormalSensitivity;
				float deltaNormal = (diffNormal.x + diffNormal.y)*_NormalSensitivity>0.1;
				return deltaDepth + deltaNormal;
			}
			float effect_GetEdge(sampler2D gBuffer, float2 uv) {
				float2 texel = 1 / _ScreenParams.xy;
				//fixed4 g0 = tex2D(gBuffer, uv + texel*float2(0, 0));
				fixed4 g1 = tex2D(gBuffer, uv + texel*float2( 0,  1));
				fixed4 g2 = tex2D(gBuffer, uv + texel*float2( 0, -1));
				fixed4 g3 = tex2D(gBuffer, uv + texel*float2(-1,  0));
				fixed4 g4 = tex2D(gBuffer, uv + texel*float2( 1,  0));
				float edge = effect_GetGBufferDifference(g2, g1);
				edge += effect_GetGBufferDifference(g3, g4);
				return edge;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv);
				fixed edge = (1 - effect_GetEdge(_CameraDepthNormalsTexture, i.uv));
				return col*edge;
			}
			ENDCG
		}
	}
}
