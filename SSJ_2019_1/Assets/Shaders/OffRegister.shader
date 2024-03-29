﻿Shader "Hidden/OffRegister"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Offset1("Offset1",Vector) = (0,0,0,0)
		_Offset2("Offset2",Vector) = (0,0,0,0)
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
			float4 _Offset1;
			float4 _Offset2;
			fixed3 cmyk2rgb(fixed4 cmyk) {
				cmyk.xyz = (cmyk.xyz * (1 - cmyk.w) + cmyk.w);
				return  1 - cmyk.xyz;
				
			}
			fixed4 rgb2cmyk(fixed3 rgb) {

				fixed4 cmyk;
				cmyk.xyz = 1 - rgb.xyz;
				cmyk.w = 1;
				cmyk.w = min(cmyk.x, min(cmyk.y, min(cmyk.z, cmyk.w)));
				cmyk.xyz = cmyk.w == 1 ? float3(0, 0, 0) : (cmyk.xyz - cmyk.w) / (1 - cmyk.w);
				return cmyk;
			}
			float GetPass(float2 offset,float2 texel, int index) {
				fixed4 col = tex2D(_MainTex, offset);
				fixed4 cmyk = rgb2cmyk(col.rgb);
				float2 subUV = fmod(offset, texel * _Offset2.w) / (texel * _Offset2.w);
				float v = saturate(1-distance(fixed2(0.5, 0.5), subUV) + cmyk[index]-.5)*0.7+0.2;
				//return cmyk[index];
				return smoothstep(.45, .55, v)*v;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				float2 texel = 1 / _ScreenParams.xy;
				float2 subUV = fmod(i.uv, texel* _Offset2.w)/ (texel * _Offset2.w);
                fixed4 col1 = tex2D(_MainTex, i.uv);
				fixed4 cmyk1 = rgb2cmyk(col1.rgb);
				float4 dot;// = distance(fixed2(0.5, 0.5), subUV) - cmyk1.w;

				dot.x = GetPass(i.uv + texel * _Offset1.xy * _Offset2.z, texel, 0);
				dot.y = GetPass(i.uv + texel * _Offset1.zw * _Offset2.z, texel, 1);
				dot.z = GetPass(i.uv + texel * _Offset2.xy * _Offset2.z, texel, 2);
				dot.w = GetPass(i.uv, texel, 3);

				fixed4 rgb = cmyk2rgb(dot).xyzz;
				float frameSize = 20;

                return rgb;
            }
            ENDCG
        }
    }
}
